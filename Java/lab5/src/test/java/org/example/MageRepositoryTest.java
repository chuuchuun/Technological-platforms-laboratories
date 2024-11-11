package org.example;
import org.junit.jupiter.api.Test;
import java.util.*;
import static org.assertj.core.api.Assertions.assertThat;
public class MageRepositoryTest {
    @Test
    public void testFindNonExistingMage(){
        MageRepository repository = new MageRepository();
        Optional res = repository.find("Not existing mage");
        assertThat(res).isEmpty();
    }
    @Test
    public void testFindExistingMage(){
        MageRepository repository = new MageRepository();
        Mage mage = new Mage("ExistingMage", 1);
        repository.save(mage);

        Optional<Mage> res = repository.find("ExistingMage");

        assertThat(res).isPresent().contains(mage);
    }

    @Test
    public void testDeleteNonExistingMage(){
        MageRepository repository = new MageRepository();
        try {
            repository.delete("NotExistingMage");
        }
        catch(IllegalArgumentException e){
            System.out.println("not found");
        }
    }

    @Test
    public void testDeleteExistingMage(){
        MageRepository repository = new MageRepository();
        Mage mage = new Mage("ExistingMage2", 1);

        repository.save(mage);
        repository.delete(mage.getName());
        Optional<Mage> res = repository.find("ExistingMage2");

        assertThat(res).isEmpty();
    }
    @Test
    public void testSaveExistingMage(){
        MageRepository repository = new MageRepository();
        Mage mage = new Mage("ExistingMage", 1);
        repository.save(mage);
        try {
            repository.save(mage);
        }
        catch(IllegalArgumentException e){
            System.out.println("bad request");
        }
    }

    @Test
    public void testSaveNonExistingMage(){
        MageRepository repository = new MageRepository();
        Mage mage = new Mage("NonExistingMage", 1);
        repository.save(mage);
        Optional<Mage> res = repository.find("NonExistingMage");
        assertThat(res).isPresent().contains(mage);
    }

}
