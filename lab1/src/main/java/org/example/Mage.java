package org.example;

import java.util.*;

public class Mage implements Comparable<Mage>{
    private String name;
    private int level;
    private double power;

    public Mage(String name, Integer level, Double power, Set<Mage> apprentices){
        this.name = name;
        this.level = level;
        this.power = power;
        this.apprentices = apprentices;
    }
    public String getName() {
        return name;
    }

    public int getLevel() {
        return level;
    }

    public double getPower() {
        return power;
    }

    public Set<Mage> getApprentices() {
        return apprentices;
    }

    private Set<Mage> apprentices;

    @Override
    public int hashCode() {
        return Objects.hash(name, level, power);
    }

    @Override
    public boolean equals(Object obj) {
        if (obj == null) {
            throw new NullPointerException();
        }
        if(obj.getClass()!= Mage.class) {
            throw new ClassCastException();
        }
        if(obj == this){
            return true;
        }
        Mage mage = (Mage) obj;
        return(mage.name.equals(name) &&
                mage.level == level &&
                mage.power == power);
    }

    @Override
    public String toString() {
        return "Mage{name='" + name + "', level=" + level + ", power=" + power + "}";
    }

    @Override
    public int compareTo(Mage o) {
        if (o == null) {
            throw new NullPointerException();
        }
        if(o.getClass()!= Mage.class) {
            throw new ClassCastException();
        }
        if(o == this){
            return 0;
        }
        Mage mage = (Mage) o;
        if(name.compareTo(mage.name) != 0) {
            return name.compareTo(mage.name);
        }
        if (Integer.compare(level, mage.level) != 0){
            Integer.compare(level, mage.level);
        }
        if(Double.compare(power, mage.power) != 0) {
            Double.compare(power, mage.power);
        }
        return 0;
    }

    public void printOut(String prefix){
        System.out.println(prefix + this);
        int number = 1;
        for(Mage app : apprentices){
            app.printOut((char) 9 + prefix + number + ".");
            number++;
        }
    }

    public int getTotalApprentices(){
        int total = apprentices.size();
        for(Mage app : apprentices) {
            total += app.getTotalApprentices();
        }
        return total;
    }

    public Map<Mage, Integer> generateStatistics (String type, Mage root ){
        Map<Mage, Integer> statisticsMap;
        if(type.compareTo("None") == 0){
            statisticsMap = new HashMap<>();
        }
        else{
            statisticsMap = new TreeMap<>();
        }
        populateMap(statisticsMap, root);
        return statisticsMap;
    }
    public void populateMap(Map<Mage, Integer> statisticsMap, Mage mage){
        statisticsMap.put(mage, mage.getTotalApprentices());
        for(Mage app : mage.apprentices ){
            populateMap(statisticsMap, app);
        }
    }
}
