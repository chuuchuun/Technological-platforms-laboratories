package org.example;

import javax.persistence.*;
@Entity
@Table(name = "mage")
public class Mage {
    @Id
    private String name;
    private int level;
    @ManyToOne (cascade = CascadeType.ALL)
    @JoinColumn(name="Tname")
    private Tower tower;

    public Mage(String name, int level, Tower tower) {
        this.name = name;
        this.level = level;
        this.tower = tower;
        this.tower.addMage(this);
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public int getLevel() {
        return level;
    }

    public void setLevel(int level) {
        this.level = level;
    }

    public Tower getTower() {
        return tower;
    }

    public void setTower(Tower tower) {
        this.tower = tower;
    }

}
