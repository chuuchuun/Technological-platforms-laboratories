package org.example;

import javax.persistence.*;

import java.util.ArrayList;
import java.util.List;

@Entity
public class Tower {
    @Id
    private String name;
    private int height;
    @OneToMany(cascade = CascadeType.ALL, mappedBy = "tower")
    private List<Mage> mages;

    public Tower(String name, int height) {
        this.name = name;
        this.height = height;
        this.mages = new ArrayList<Mage>();
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public int getHeight() {
        return height;
    }

    public void setHeight(int height) {
        this.height = height;
    }

    public List<Mage> getMages() {
        return mages;
    }
    public void setMages(List<Mage> mages) {
        this.mages = mages;
    }
    public void addMage(Mage mage){
        this.mages.add(mage);
    }
}
