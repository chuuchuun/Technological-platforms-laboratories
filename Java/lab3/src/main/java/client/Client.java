package client;

import org.example.Message;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.net.Socket;
import java.util.Scanner;

public class Client {
    public static void main(String args[]) throws IOException, ClassNotFoundException {
        Socket clientSocket = new Socket("localHost", 1111);

        Scanner scanner = new Scanner(System.in);

        ObjectOutputStream out = new ObjectOutputStream(clientSocket.getOutputStream());
        ObjectInputStream in = new ObjectInputStream(clientSocket.getInputStream());

        String id = (String) in.readObject();
        System.out.println("Id: " + id);

        String first = (String) in.readObject();
        System.out.println(first);

        int messageNumber = Integer.parseInt(scanner.nextLine());
        out.writeInt(messageNumber);
        out.flush();

        String second = (String) in.readObject();
        System.out.println(second);

        for(int i = 0; i < messageNumber; i++){
            String content = scanner.nextLine();
            Message temp = new Message(i, content, id);
            out.writeObject(temp);
            out.flush();
        }
        String third = (String) in.readObject();
        System.out.println(third);

        out.close();
        in.close();
        clientSocket.close();
    }
}
