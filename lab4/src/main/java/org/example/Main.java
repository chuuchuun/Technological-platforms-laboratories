package org.example;

import javax.persistence.*;


import java.util.List;

public class Main {
    private static EntityManagerFactory entities = Persistence.createEntityManagerFactory("jpa-hibernate-example");

    public static void main(String[] args) {
        EntityManager entityManager = entities.createEntityManager();
        EntityTransaction entityTransaction = entityManager.getTransaction();

        entityTransaction.begin();
        Tower t1 = new Tower("Tower1", 20);
        Tower t2 = new Tower("Tower2", 10);
        Tower t3 = new Tower("Tower3", 28);

        Mage mage1 = new Mage("Mage1", 4, t2);
        Mage mage2 = new Mage("Mage2", 5, t1);
        Mage mage3 = new Mage("Mage3", 10, t3);


        entityManager.persist(mage1);
        entityManager.persist(mage2);
        entityManager.persist(mage3);

        entityManager.persist(t1);
        entityManager.persist(t2);
        entityManager.persist(t3);


        entityTransaction.commit();

        entityTransaction.begin();

        List<Tower> towers = entityManager.createQuery("SELECT t from Tower t", Tower.class).getResultList();
        for(Tower tower : towers){
            System.out.println("Tower: " + tower.getName() + " height: " + tower.getHeight() + " with mages: " + tower.getMages());
        }
        List<Mage> mages = entityManager.createQuery("SELECT m from Mage m", Mage.class).getResultList();
        for(Mage mage : mages){
            System.out.println("Mage: " + mage.getName() + " level: " + mage.getLevel() + " in tower: " + mage.getTower().getName());
        }
        Tower toDelete = entityManager.find(Tower.class, "Tower2");
        if(toDelete != null){
            toDelete.setMages(null);
            entityManager.remove(toDelete);
            System.out.println("Deleted " + toDelete.getName());
        }

        Mage toDelete2 = entityManager.find(Mage.class, "Mage1");
        if(toDelete2 != null){
            toDelete2.setTower(null);
            entityManager.remove(toDelete2);
            System.out.println("Deleted " + toDelete2.getName());
        }
        List<Mage> mages2 = entityManager.createQuery("SELECT m from Mage m where m.level > 3").getResultList();
        for(Mage mage : mages2){
            System.out.println("Mage: " + mage.getName() + " level: " + mage.getLevel() + " in tower: " + mage.getTower().getName());
        }
        List<Tower> towers2 = entityManager.createQuery("SELECT t from Tower t where t.height > 21", Tower.class).getResultList();
        for(Tower tower : towers2){
            System.out.println("Tower: " + tower.getName() + " height: " + tower.getHeight() );
        }
        entityTransaction.commit();

        entityManager.close();
        entities.close();
    }
}