package homework;

import java.util.logging.Logger;

// Lớp cha
class Animal {
    void speak() {
        Logger.getLogger(Animal.class.getName()).info("Animal speaks");
    }
}

// Lớp con
class Dog extends Animal {
    private static final Logger logger = Logger.getLogger(Dog.class.getName());

    @Override
    void speak() {
        logger.info("Dog barks");
    }
}

// Class chính để kiểm tra
public class MissingOverrideAnnotationExample {
    public static void main(String[] args) {
        Animal animal = new Dog();
        animal.speak(); // In ra: Dog barks (nếu override đúng)
    }
}
