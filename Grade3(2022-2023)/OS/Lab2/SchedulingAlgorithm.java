// Run() is called from Scheduling.main() and is where
// the scheduling algorithm written by the user resides.
// User modification should occur within the Run() function.

import java.util.Vector;
import java.io.*;

public class SchedulingAlgorithm {

  public static Results Run(int runtime, Vector processVector, Results result) {
    int i = 0;
    int comptime = 0;
    int currentProcess = 0;
    int previousProcess = 0;
    int size = processVector.size();

    int completed = 0;
    boolean isNeedToCheckForProcessSwitch = false;
    String resultsFile = "Summary-Processes";

    result.schedulingType = "Preemptive";
    result.schedulingName = "Earliest deadline first";
    try {
      PrintStream out = new PrintStream(new FileOutputStream(resultsFile));
      out.println(size);
      currentProcess = NextProcess(processVector);
      sProcess process = (sProcess) processVector.elementAt(currentProcess);
      out.println("Process: " + currentProcess + " registered... "+GetProcessInfo(process));
      while (comptime < runtime) {
        isNeedToCheckForProcessSwitch = NextStep(processVector, currentProcess);

        if(currentProcess != -1){
          if (process.cpudone == process.cputime) {
            completed++;
            out.println("Process: " + currentProcess + " completed... "  +GetProcessInfo(process));

            if (completed == size) {
              result.compuTime = comptime;
              out.println("All processes completed!");
              out.close();
              return result;
            }
          }else if (process.timebeforeblocking == process.timebeforeblockingdone) {
            process.numblocked++;
            out.println("Process: " + currentProcess + " I/O blocked... " + GetProcessInfo(process));
          }
        }

        if(currentProcess == -1){
          currentProcess = NextProcess(processVector);
          if (currentProcess > 0) {
            process = (sProcess) processVector.elementAt(currentProcess);
            out.println("Process: " + currentProcess + " registered... "+GetProcessInfo(process));
          }
        }

        if(isNeedToCheckForProcessSwitch){
          previousProcess = currentProcess;
          currentProcess = NextProcess(processVector);

          if(previousProcess != currentProcess){
            if(currentProcess == -1){
              out.println("No available process");
            }else if(previousProcess == -1){
              process = (sProcess) processVector.elementAt(currentProcess);
              out.println("Process: " + currentProcess + " registered... "+GetProcessInfo(process));
            }else{
              out.println("Switched from process: " + previousProcess + " " + GetProcessInfo(process));
              process = (sProcess) processVector.elementAt(currentProcess);
              out.println("Switched to process: " + currentProcess + " " + GetProcessInfo(process));
            }
          }
        }

        comptime++;
      }
      out.println("Comp time equals to run time !");
      out.close();
    } catch (IOException e) { /* Handle exceptions */ }
    result.compuTime = comptime;
    return result;
  }

  private static int NextProcess(Vector processVector){
    int nextProcessIndex = -1;
    int deadlineTimeDistance = -1;

    for (int i = 0; i < processVector.size(); i++){
      sProcess process = (sProcess) processVector.elementAt(i);
      if(process.cpudone < process.cputime && process.timebeforeblocking != process.timebeforeblockingdone){ //process isn`t complete or blocked
        int deadlineTimeDistanceCurrent = (process.timebeforeblocking - process.timebeforeblockingdone) + process.blockingtime;
        if(deadlineTimeDistanceCurrent < deadlineTimeDistance || nextProcessIndex < 0){
          nextProcessIndex = i;
          deadlineTimeDistance = deadlineTimeDistanceCurrent;
        }
      }
    }

    return nextProcessIndex;
  }

  private static boolean NextStep(Vector processVector, int currentProcess){

    boolean isNeedToCheckForProcessSwitch = false;

    for (int i = 0; i < processVector.size(); i++){
      sProcess process = (sProcess) processVector.elementAt(i);

      if(i == currentProcess){
        //If it is current process increase time before blocking done
        process.timebeforeblockingdone++;
        process.cpudone++;
        if(process.timebeforeblockingdone == process.timebeforeblocking){
          //Current process is locked
          isNeedToCheckForProcessSwitch = true;
        }
        if (process.cpudone == process.cputime){
          //Current process is complete
          isNeedToCheckForProcessSwitch = true;
        }
      }
      else if(process.timebeforeblocking == process.timebeforeblockingdone){
        //If process now is blocking increase blocking time done
        process.blockingtimedone++;
        if(process.blockingtimedone == process.blockingtime){
          //Unlock process
          process.timebeforeblockingdone = 0;
          process.blockingtimedone = 0;
          isNeedToCheckForProcessSwitch = true;
        }
      }
    }

    return isNeedToCheckForProcessSwitch;
  }

  private static String GetProcessInfo(sProcess process){
   return ("( cpu time: " + process.cpudone + "/"  + process.cputime +" time before blocking: "+ process.timebeforeblockingdone + "/" + process.timebeforeblocking + " time after blocking: "+process.blockingtimedone +"/"+ process.blockingtime + ")");
  }
}
