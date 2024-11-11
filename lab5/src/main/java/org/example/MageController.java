package org.example;

import java.util.Optional;

public class MageController {
    private MageRepository repository;
    public MageController(MageRepository repository) {
       this.repository = repository;
    }
    public String find(String name) {
        Optional<Mage> mageOptional = repository.find(name);
        if(mageOptional.isPresent()){
            Mage mage = mageOptional.get();
            return "Name: " + mage.getName() + " level: " + mage.getLevel() +".";
        }
        else{
            return "not found";
        }
    }
    public String delete(String name) {
        try {
            repository.delete(name);
            return "done";
        }
        catch(IllegalArgumentException e){
            return "not found";
        }
    }
    public String save(String name, String level) {
        int mage_level = Integer.parseInt(level);
        Mage mage = new Mage(name, mage_level);
        repository.save(mage);
        Optional<Mage> mageOptional = repository.find(name);
        if(mageOptional.isPresent()){
            return "done";
        }
        else{
            return "bad request";
        }
    }
}
