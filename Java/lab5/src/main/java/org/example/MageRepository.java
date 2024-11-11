package org.example;

import java.util.*;

public class MageRepository {
    private Collection<Mage> collection = new ArrayList<>();
    public Optional<Mage> find(String name) {
        for(Mage mage : collection){
            if(name.compareTo(mage.getName()) == 0){
                return Optional.of(mage);
            }
        }
        return Optional.empty();
    }
    public void delete(String name) {
        Boolean deleted = false;
        for(Mage mage : collection){
            if(name.equals(mage.getName())){
                Optional.of(collection.remove(mage));
                deleted = true;
            }
        }
        if(!deleted) {
            throw new IllegalArgumentException("Object with name " + name + "does not exist.");
        }
    }
    public void save(Mage mage) {
        for(Mage mage1 : collection){
            if(mage.getName().equals(mage1.getName())){
                throw new IllegalArgumentException("Object with name "+ mage.getName() + "already exists.");
            }
        }
        collection.add(mage);
    }
}
