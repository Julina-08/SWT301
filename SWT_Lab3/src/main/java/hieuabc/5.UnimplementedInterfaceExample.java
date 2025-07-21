interface Drawable {
    void draw();
}

class Circle implements Drawable {
    // Missing draw() implementation → compile error
}
