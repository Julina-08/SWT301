package homework;

import java.util.logging.Logger;

// Class User tuân thủ nguyên tắc đóng gói
class User {
    private static final Logger logger = Logger.getLogger(User.class.getName());

    private final String name;
    private final int age;

    // Constructor
    public User(String name, int age) {
        this.name = name;
        this.age = age;
    }

    // Getter
    public String getName() {
        return name;
    }

    // Hiển thị thông tin
    public void display() {
        logger.info("Name: " + name + ", Age: " + age);
    }
}

// Class chính để sử dụng User
public class ViolationOfEncapsulationExample {
    public static void main(String[] args) {
        User user = new User("Alice", 25);
        user.display();
    }
}
