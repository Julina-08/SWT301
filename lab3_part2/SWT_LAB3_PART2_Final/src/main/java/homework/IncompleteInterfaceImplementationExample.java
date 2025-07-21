package homework;

import java.util.logging.Logger;

// Interface Shape với 2 phương thức
interface Shape {
    void draw();
    void resize();
}

// Lớp Square triển khai đầy đủ interface
class Square implements Shape {
    private static final Logger logger = Logger.getLogger(Square.class.getName());

    @Override
    public void draw() {
        logger.info("Drawing square");
    }

    @Override
    public void resize() {
        logger.info("Resizing square");
    }
}

// Class chính để sử dụng
public class IncompleteInterfaceImplementationExample {
    public static void main(String[] args) {
        Shape shape = new Square();
        shape.draw();
        shape.resize();
    }
}
