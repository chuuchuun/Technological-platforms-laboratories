package org.example;

import java.util.Comparator;

public class MageComparator implements Comparator<Mage> {
    @Override
    public int compare(Mage o1, Mage o2) {
        if (o1 == null || o2 == null) {
            throw new NullPointerException();
        }
        if(o1 == o2){
            return 0;
        }
        if (Integer.compare(o1.getLevel(), o2.getLevel()) != 0){
            return Integer.compare(o1.getLevel(), o2.getLevel());
        }
        if(o1.getName().compareTo(o2.getName()) != 0) {
            return o1.getName().compareTo(o2.getName());
        }
        if(Double.compare(o1.getPower(), o2.getPower()) != 0) {
            return Double.compare(o1.getPower(), o2.getPower());
        }
        return 0;
    }
}
