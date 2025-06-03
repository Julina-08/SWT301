//TIP To <b>Run</b> code, press <shortcut actionId="Run"/> or
// click the <icon src="AllIcons.Actions.Execute"/> icon in the gutter.
public class Main {
    public static void main(String[] args) {
        //TIP Press <shortcut actionId="ShowIntentionActions"/> with your caret at the highlighted text
        // to see how IntelliJ IDEA suggests fixing it.
        System.out.printf("Hello and welcome!");

        for (int i = 1; i <= 5; i++) {
            //TIP Press <shortcut actionId="Debug"/> to start debugging your code. We have set one <icon src="AllIcons.Debugger.Db_set_breakpoint"/> breakpoint
            // for you, but you can always add more by pressing <shortcut actionId="ToggleLineBreakpoint"/>.
            System.out.println("i = " + i);
        }
    }
}

//public class InsuranceClaim {
//    String ClaimId;
//    double amount;
//    String claimStatus;
//
//    InsuranceClaim(String id, double claimAmount) {
//        ClaimId = id;
//        amount = claimAmount;
//        claimStatus = "Pending";
//    }
//
//    boolean ProcessClaim(String statusUpdate) {
//        if (claimStatus == "Pending") {
//            claimStatus = statusUpdate;
//            return true;
//        }
//        return false;
//    }
//
//    double calculatePayout(){
//        if (claimStatus.equals("Approved")) {
//            return amount*0.85;
//        } else
//            return 0;
//    }
//    void updateClaimAmount(double newAmount) {
//        amount = newAmount;
//    }
//}
