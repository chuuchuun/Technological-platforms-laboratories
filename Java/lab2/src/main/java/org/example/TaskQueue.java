package org.example;

import java.util.LinkedList;
import java.util.Queue;

public class TaskQueue {
    private Queue<String> taskQueue = new LinkedList<String>();

    public synchronized void addTask(int id, int N){
        String task = id + " " + N;
        taskQueue.add(task);
        notify();
    }

    public synchronized String getTask() throws InterruptedException {
        while(taskQueue.isEmpty()){
            wait();
        }
        return taskQueue.poll();
    }
}
