import example.Calculator;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.*;

class CalculatorTest {
    Calculator calc = new Calculator();
    @Test
    void testAddition() {
        assertEquals(5, calc.add(2, 3), "addition should return 5");
    }

    @Test
    void testDivision() {
        assertEquals(2, calc.divide(6, 3));
    }

    @Test
    void testDivideByZero() {
        Exception e = assertThrows(ArithmeticException.class, ()
                -> {
            calc.divide(10, 0);
        });
        assertEquals("cannot divide by zero", e.getMessage());
    }
}
