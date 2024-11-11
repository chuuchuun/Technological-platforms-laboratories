package org.example;

import java.util.HashMap;
import java.util.Map;

public class ResultQueue {
    private Map<String, String> resultQueue = new HashMap<>();

    public synchronized void addResult(String id, String result){
        resultQueue.put(id, result);
    }
    public void printOut(){
        for(String id : resultQueue.keySet()){
            System.out.println(id + " " + resultQueue.get(id));
        }
    }
}
