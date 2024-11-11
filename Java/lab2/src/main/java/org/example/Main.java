package org.example;
import java.util.Scanner;

public class Main {
    public static void main(String[] args) {
        TaskQueue taskQueue = new TaskQueue();
        ResultQueue resultQueue = new ResultQueue();

        int counter = Integer.parseInt(args[0]);
        Thread[] threads = new Thread[counter];

        for(int i = 0; i < counter; i++){
            TaskService task = new TaskService(taskQueue, resultQueue);
            threads[i] = new Thread(task);
            threads[i].start();
        }
        Scanner scanner = new Scanner(System.in);
        boolean running = true;
        int count = 0;
        while(running){
            System.out.println("To add a task please enter a number\nTo exit press 'q'\nTo print out the tasks press 't'");
            String input = scanner.nextLine();
            if(input.compareTo("q") == 0){
                resultQueue.printOut();
                running = false;
            }
            else if (input.compareTo("t") == 0){
                resultQueue.printOut();
            }
            else{
                int task = Integer.parseInt(input);
                count++;
                taskQueue.addTask(count, task);
            }
        }
        for(int i =0 ; i < counter; i++){
            threads[i].interrupt();
        }
        scanner.close();
    }
}