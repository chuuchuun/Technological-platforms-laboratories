package org.example;

import org.apache.commons.lang3.tuple.Pair;

import javax.imageio.ImageIO;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.List;
import java.util.concurrent.ForkJoinPool;
import java.util.stream.Collectors;

public class Main {

    public static String INPUT_DIRECTORY;
    public static String OUTPUT_DIRECTORY;
    private static final int THREAD_POOL_SIZE = 2;

    public static void main(String[] args) {
        INPUT_DIRECTORY = args[0];
        OUTPUT_DIRECTORY = args[1];
        try {
            List<Pair<String, BufferedImage>> imagePairs = Files.list(Path.of(INPUT_DIRECTORY))
                    .parallel()
                    .map(Main::loadImagePair)
                    .collect(Collectors.toList());
            System.out.println("Loaded " + imagePairs.size() + " images");
            long time = System.currentTimeMillis();
            ForkJoinPool customThreadPool = new ForkJoinPool(THREAD_POOL_SIZE);
            customThreadPool.submit(() ->
                    imagePairs.parallelStream()
                            .map(Main::transformImagePair)
                            .forEach(Main::saveImage)
            ).get();
            System.out.println("Program has ended in "+ (System.currentTimeMillis() - time) + " milliseconds.");
        } catch (Exception e) {
            System.out.println("Failed to process images: " + e.getMessage());
            e.printStackTrace();
        }
    }

    private static Pair<String, BufferedImage> loadImagePair(Path path) {
        try {
            BufferedImage image = ImageIO.read(path.toFile());
            String name = path.getFileName().toString();
            return Pair.of(name, image);
        } catch (IOException e) {
            System.out.println("Failed to load image: " + path);
            e.printStackTrace();
            return null;
        }
    }

    private static Pair<String, BufferedImage> transformImagePair(Pair<String, BufferedImage> pair) {
        String name = pair.getLeft();
        BufferedImage image = pair.getRight();
        if (image == null) {
            System.out.println("Failed to transform image: " + name + " - Image is null");
            return null;
        }
        BufferedImage transformedImage = new BufferedImage(image.getWidth(), image.getHeight(), BufferedImage.TYPE_INT_RGB);
        for (int x = 0; x < image.getWidth(); x++) {
            for (int y = 0; y < image.getHeight(); y++) {
                int rgb = image.getRGB(x, y);
                Color color = new Color(rgb);
                int red = color.getRed();
                int blue = color.getBlue();
                int green = color.getGreen();
                Color outColor = new Color(red, blue, green);
                transformedImage.setRGB(x, image.getHeight() -y - 1, outColor.getRGB());
            }
        }

        return Pair.of(name, transformedImage);
    }

    private static void saveImage(Pair<String, BufferedImage> pair) {
        String name = pair.getLeft();
        int lastIndex = name.lastIndexOf('.');
        if (lastIndex == -1) {
            name += "_modified";
        }
        String baseName = name.substring(0, lastIndex);
        String extension = name.substring(lastIndex);
        name =  baseName + "_modified" + extension;
        BufferedImage image = pair.getRight();
        if (image == null) {
            System.out.println("Failed to save image: " + name + " - Transformed image is null");
            return;
        }
        Path outputPath = Path.of(OUTPUT_DIRECTORY, name);
        try {
            ImageIO.write(image, "png", outputPath.toFile());
            System.out.println("Saved image: " + outputPath);
        } catch (IOException e) {
            System.out.println("Failed to save image: " + outputPath);
            e.printStackTrace();
        }
    }
}
