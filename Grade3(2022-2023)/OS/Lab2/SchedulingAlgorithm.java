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
      currentProcess = NextProcess(processVector);
      sProcess process = (sProcess) processVector.elementAt(currentProcess);
      out.println("Process: " + currentProcess + " registered... "+GetProcessInfo(process, comptime));

      while (comptime < runtime) {
        comptime++;

        isNeedToCheckForProcessSwitch = NextStep(processVector, currentProcess, comptime);

        if(currentProcess != -1){
          if (process.cpudone == process.cputime) {
            completed++;
            out.println("Process: " + currentProcess + " completed... "  +GetProcessInfo(process, comptime));

            if (completed == size) {
              result.compuTime = comptime;
              out.println("All processes completed!");
              out.close();
              return result;
            }
          }else if (process.processingtimedone == process.processingtime) {
            out.println("Process: " + currentProcess + " is unable... " + GetProcessInfo(process, comptime));
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
              out.println("Process: " + currentProcess + " registered... "+GetProcessInfo(process, comptime));
            }else{
              out.println("Switched from process: " + previousProcess + " " + GetProcessInfo(process, comptime));
              process = (sProcess) processVector.elementAt(currentProcess);
              out.println("Switched to process: " + currentProcess + " " + GetProcessInfo(process, comptime));
            }
          }
        }
      }
      out.println("Comp time equals to run time !");
      out.close();
    } catch (IOException e) { /* Handle exceptions */ }
    result.compuTime = comptime;
    return result;
  }

  private static int NextProcess(Vector processVector){
    int nextProcessIndex = -1;
    int deadline = -1;

    for (int i = 0; i < processVector.size(); i++){
      sProcess process = (sProcess) processVector.elementAt(i);
      if(process.cpudone < process.cputime && process.processingtimedone != process.processingtime){ //process isn`t complete or unable
        if(process.deadline < deadline || nextProcessIndex < 0){
          nextProcessIndex = i;
          deadline = process.deadline;
        }
      }
    }

    return nextProcessIndex;
  }

  private static boolean NextStep(Vector processVector, int currentProcess, int comptime){

    boolean isNeedToCheckForProcessSwitch = false;

    for (int i = 0; i < processVector.size(); i++){
      sProcess process = (sProcess) processVector.elementAt(i);

      if(i == currentProcess){
        process.cpudone++;
        process.processingtimedone++;

        if (process.cpudone == process.cputime){
          isNeedToCheckForProcessSwitch = true;
        }else if(process.processingtimedone == process.processingtime){
          isNeedToCheckForProcessSwitch = true;
        }
      }
      else if(process.deadline == comptime){
        process.processingtimedone = 0;
        process.deadline += process.period;
        isNeedToCheckForProcessSwitch = true;
      }
    }

    return isNeedToCheckForProcessSwitch;
  }

  private static String GetProcessInfo(sProcess process, int comptime){
   return ("( cpu time: " + process.cpudone + "/"  + process.cputime +" processing time: "+ process.processingtimedone + "/" + process.processingtime + " period time: " + process.period + " deadline: "+comptime+"/"+process.deadline+")");
  }
}
