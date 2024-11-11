package org.example;

import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.TreeSet;

// Press Shift twice to open the Search Everywhere dialog and type `show whitespaces`,
// then press Enter. You can now see whitespace characters in your code.
public class Main {
    public static void main(String[] args) {
        String sorting = args[0];
        if(sorting.compareTo("None") == 0){
            Mage child1 = new Mage("child1", 2, 1.0, new HashSet<>());
            Mage child2 = new Mage("child2", 8, 1.0, new HashSet<>());
            Mage child3 = new Mage("child3", 5, 1.0, new HashSet<>());
            Mage child4 = new Mage("child4", 7, 1.0, new HashSet<>());
            Mage child5 = new Mage("child5", 6, 1.0, new HashSet<>());

            Mage parent1 = new Mage("parent1", 2, 1.0, new HashSet<>(List.of(child1)));
            Mage parent2 = new Mage("parent2", 4, 1.0, new HashSet<>(List.of(child2, child3)));
            Mage parent3 = new Mage("parent3", 5, 1.0, new HashSet<>(List.of(child4, child5)));

            Mage grandparent1 = new Mage("grandparent1", 6, 1.0, new HashSet<>(List.of(parent1)));
            Mage grandparent2 = new Mage("grandparent2", 3, 1.0, new HashSet<>(List.of(parent2, parent3)));

            Mage root = new Mage ("root", 1, 1.0, new HashSet<>(List.of(grandparent1, grandparent2)));
            root.printOut("");
            Map<Mage, Integer> stats = root.generateStatistics(sorting, root);
            for(Map.Entry<Mage, Integer> entry : stats.entrySet()){
                System.out.println(entry.getKey() + " has a total of " + entry.getValue() + " children");
            }
        }
        else if(sorting.compareTo("Natural") == 0){
            Mage child1 = new Mage("child1", 2, 1.0, new TreeSet<>());
            Mage child2 = new Mage("child2", 8, 1.0, new TreeSet<>());
            Mage child3 = new Mage("child3", 5, 1.0, new TreeSet<>());
            Mage child4 = new Mage("child4", 7, 1.0, new TreeSet<>());
            Mage child5 = new Mage("child5", 6, 1.0, new TreeSet<>());

            Mage parent1 = new Mage("parent1", 2, 1.0, new TreeSet<>(List.of(child1)));
            Mage parent2 = new Mage("parent2", 4, 1.0, new TreeSet<>(List.of(child2, child3)));
            Mage parent3 = new Mage("parent3", 5, 1.0, new TreeSet<>(List.of(child4, child5)));

            Mage grandparent1 = new Mage("grandparent1", 6, 1.0, new TreeSet<>(List.of(parent1)));
            Mage grandparent2 = new Mage("grandparent2", 3, 1.0, new TreeSet<>(List.of(parent2, parent3)));

            Mage root = new Mage ("root", 1, 1.0, new TreeSet<>(List.of(grandparent1, grandparent2)));
            root.printOut("");
            Map<Mage, Integer> stats = root.generateStatistics(sorting, root);
            for(Map.Entry<Mage, Integer> entry : stats.entrySet()){
                System.out.println(entry.getKey() + " has a total of " + entry.getValue() + " children");
            }
        }
        else if(sorting.compareTo("Alternative") == 0) {
            Mage child1 = new Mage("child1", 2, 1.0, new TreeSet<>());
            Mage child2 = new Mage("child2", 8, 1.0, new TreeSet<>());
            Mage child3 = new Mage("child3", 5, 1.0, new TreeSet<>());
            Mage child4 = new Mage("child4", 7, 1.0, new TreeSet<>());
            Mage child5 = new Mage("child5", 6, 1.0, new TreeSet<>());

            TreeSet<Mage> parent1Set = new TreeSet<>(new MageComparator());
            parent1Set.addAll(List.of(child1));
            Mage parent1 = new Mage("parent1", 2, 1.0, parent1Set);
            TreeSet<Mage> parent2Set = new TreeSet<>(new MageComparator());
            parent2Set.addAll(List.of(child2, child3));
            Mage parent2 = new Mage("parent2", 4, 1.0, parent2Set);
            TreeSet<Mage> parent3Set = new TreeSet<>(new MageComparator());
            parent3Set.addAll(List.of(child4, child5));
            Mage parent3 = new Mage("parent3", 5, 1.0, parent3Set);

            TreeSet<Mage> grandparent1Set = new TreeSet<>(new MageComparator());
            grandparent1Set.addAll(List.of(parent1));
            Mage grandparent1 = new Mage("grandparent1", 6, 1.0, grandparent1Set);
            TreeSet<Mage> grandparent2Set = new TreeSet<>(new MageComparator());
            grandparent2Set.addAll(List.of(parent2, parent3));
            Mage grandparent2 = new Mage("grandparent2", 3, 1.0, grandparent2Set);

            TreeSet<Mage> rootSet = new TreeSet<>(new MageComparator());
            rootSet.addAll(List.of(grandparent1, grandparent2));
            Mage root = new Mage("root", 1, 1.0, rootSet);
            root.printOut("");
            Map<Mage, Integer> stats = root.generateStatistics(sorting, root);
            for(Map.Entry<Mage, Integer> entry : stats.entrySet()){
                System.out.println(entry.getKey() + " has a total of " + entry.getValue() + " children");
            }
        }
    }
}