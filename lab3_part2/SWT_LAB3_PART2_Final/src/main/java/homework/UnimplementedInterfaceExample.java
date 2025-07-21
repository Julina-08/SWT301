package homework;

import java.util.logging.Logger;

// Interface với phương thức draw
interface Drawable {
    void draw();
}

// Circle triển khai Drawable
class Circle implements Drawable {
    private static final Logger logger = Logger.getLogger(Circle.class.getName());

    @Override
    public void draw() {
        logger.info("Drawing a circle");
    }
}

// Lớp chính để chạy chương trình
public class UnimplementedInterfaceExample {
    public static void main(String[] args) {
        Drawable shape = new Circle(); // tạo đối tượng Circle thông qua interface
        shape.draw(); // gọi phương thức draw, ghi log
    }
}
