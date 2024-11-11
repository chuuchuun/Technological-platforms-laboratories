package org.example;

public class TaskService implements Runnable{
    private TaskQueue taskQueue;
    private ResultQueue resultQueue;

    TaskService(TaskQueue tqueue, ResultQueue rqueue){
        this.taskQueue = tqueue;
        this.resultQueue = rqueue;
    }
    @Override
    public void run(){
        while(true){
            try{
                String task = taskQueue.getTask();
                int id_placement = task.indexOf(" ");
                int id = Integer.parseInt(task.substring(0, id_placement));
                int task_value = Integer.parseInt(task.substring(id_placement + 1));
                double sum = 0;
                for(int i = 1;i <= task_value; i++){
                    sum += 4 * (Math.pow(-1, i - 1) / (2*i - 1));
                    String result = sum + " " + (double)i*100 /task_value;
                    resultQueue.addResult(Integer.toString(id),result);
                }
                System.out.println("Task number "+ Integer.toString(id) + " has been finished.");
                resultQueue.printOut();
            } catch (InterruptedException e) {
                throw new RuntimeException(e);
            }
        }

    }
}
