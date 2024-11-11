package org.example;

import java.io.Serializable;

public class Message implements Serializable {
    public int getNumber() {
        return number;
    }

    public void setNumber(int number) {
        this.number = number;
    }

    private int number;

    public String getContent() {
        return content;
    }

    public void setContent(String content) {
        this.content = content;
    }

    private String content;
    private String id;
    public Message(int number, String content, String id){
        this.number = number;
        this.content = content;
        this.id = id;
    }
    @Override
    public String toString(){
        return "Message: number = " + number + " content = '" + content + "' from user with id = " + id;
    }
}
