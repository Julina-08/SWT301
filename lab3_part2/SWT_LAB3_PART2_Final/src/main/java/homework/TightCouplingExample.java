package homework;

import java.util.logging.Logger;

// Interface để giảm coupling
interface OutputDevice {
    void print(String message);
}

// Triển khai cụ thể cho OutputDevice
class Printer implements OutputDevice {
    private static final Logger logger = Logger.getLogger(Printer.class.getName());

    @Override
    public void print(String message) {
        logger.info(message);
    }
}

// Lớp Report giờ phụ thuộc vào abstraction OutputDevice
class Report {
    private final OutputDevice printer;

    public Report(OutputDevice printer) {
        this.printer = printer;
    }

    public void generate() {
        printer.print("Generating report...");
    }
}

// Lớp main để chạy
public class TightCouplingExample {
    public static void main(String[] args) {
        OutputDevice printer = new Printer();
        Report report = new Report(printer);
        report.generate(); // gọi để in report
    }
}
