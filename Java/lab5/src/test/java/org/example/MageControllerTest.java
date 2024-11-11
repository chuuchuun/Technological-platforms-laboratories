package org.example;
import org.junit.jupiter.api.Test;
import org.mockito.Mockito;

import java.util.*;
import static org.assertj.core.api.Assertions.assertThat;
public class MageControllerTest {
    @Test
    public void testFindNonExistingMage(){

        MageRepository repository = Mockito.mock(MageRepository.class);
        MageController controller = new MageController(repository);

        String res = controller.find("Not existing mage");
        assertThat(res).isEqualTo("not found");
    }
    @Test
    public void testFindExistingMage(){
        MageRepository repository = Mockito.mock(MageRepository.class);
        MageController controller = new MageController(repository);
        Mage mage = new Mage("ExistingMage", 1);
        controller.save(mage.getName(), mage.getLevel());
        String res = controller.find("ExistingMage");
        assertThat(res).isEqualTo("done");
    }

    @Test
    public void testDeleteNonExistingMage(){
        MageRepository repository = Mockito.mock(MageRepository.class);
        MageController controller = new MageController(repository);

        String res = controller.delete("NotExistingMage");

        assertThat(res).isEqualTo("not found");
    }

    @Test
    public void testDeleteExistingMage(){
        MageRepository repository = Mockito.mock(MageRepository.class);
        MageController controller = new MageController(repository);
        Mage mage = new Mage("ExistingMage1", 1);
        String res = controller.delete("ExistingMage1");

        assertThat(res).isEqualTo("done");
    }
    @Test
    public void testSaveExistingMage(){
        MageRepository repository = Mockito.mock(MageRepository.class);
        MageController controller = new MageController(repository);
        Mage mage = new Mage("ExistingMage", 1);
        controller.save(mage.getName(), mage.getLevel());
        String res = controller.save(mage.getName(), mage.getLevel());
        assertThat(res).isEqualTo("bad request");
    }

    @Test
    public void testSaveNonExistingMage(){
        MageRepository repository = Mockito.mock(MageRepository.class);
        MageController controller = new MageController(repository);
        Mage mage = new Mage("NonExistingMage", 1);
        String res = controller.save(mage.getName(), mage.getLevel());
        assertThat(res).isEqualTo("done");
    }

}
