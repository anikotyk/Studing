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
    String resultsFile = "Summary-Processes";

    result.schedulingType = "Preemptive";
    result.schedulingName = "Earliest deadline first";
    try {
      //BufferedWriter out = new BufferedWriter(new FileWriter(resultsFile));
      //OutputStream out = new FileOutputStream(resultsFile);
      PrintStream out = new PrintStream(new FileOutputStream(resultsFile));
      currentProcess = NextProcess(processVector, currentProcess);
      sProcess process = (sProcess) processVector.elementAt(currentProcess);
      out.println("Process: " + currentProcess + " registered... (" + process.cputime + " " + process.ioblocking + " " + process.ionext + " " + process.cpudone + ")");
      while (comptime < runtime) {
        int prevCur = currentProcess;
        currentProcess = NextProcess(processVector, currentProcess);
        if(prevCur!=currentProcess){
          out.println("SWITCHED FROM PROCESS: " + prevCur + " completed... (" + process.cputime + " " + process.ioblocking + " " + process.ionext + " " + process.cpudone + ")");
          process = (sProcess) processVector.elementAt(currentProcess);
          out.println("SWITCHED TO PROCESS: " + currentProcess + " completed... (" + process.cputime + " " + process.ioblocking + " " + process.ionext + " " + process.cpudone + ")");
        }

        if (process.cpudone == process.cputime) {
          completed++;
          out.println("Process: " + currentProcess + " completed... (" + process.cputime + " " + process.ioblocking + " " + process.ionext + " " + process.cpudone + ")");
          if (completed == size) {
            result.compuTime = comptime;
            out.close();
            return result;
          }
          currentProcess = NextProcess(processVector, currentProcess);
          process = (sProcess) processVector.elementAt(currentProcess);
          out.println("Process: " + currentProcess + " registered... (" + process.cputime + " " + process.ioblocking + " " + process.ionext + " " + process.cpudone + ")");
        }      
        if (process.ioblocking == process.ionext) {
          out.println("Process: " + currentProcess + " I/O blocked... (" + process.cputime + " " + process.ioblocking + " " + process.ionext + " " + process.cpudone + ")");
          process.numblocked++;
          process.ionext = 0; 
          previousProcess = currentProcess;
          for (i = size - 1; i >= 0; i--) {
            process = (sProcess) processVector.elementAt(i);
            if (process.cpudone < process.cputime && previousProcess != i) { 
              currentProcess = i;
            }
          }
          process = (sProcess) processVector.elementAt(currentProcess);
          out.println("Process: " + currentProcess + " registered... (" + process.cputime + " " + process.ioblocking + " " + process.ionext + " " + process.cpudone + ")");
        }        
        process.cpudone++;       
        if (process.ioblocking > 0) {
          process.ionext++;
        }
        comptime++;
      }
      out.close();
    } catch (IOException e) { /* Handle exceptions */ }
    result.compuTime = comptime;
    return result;
  }

  private static int NextProcess(Vector processVector, int currentProcess){
    sProcess process = (sProcess) processVector.elementAt(currentProcess);
    if(process.ioblocking > process.ionext) {
      currentProcess = -1;
    }

    int nextProcessIndex = currentProcess;
    int deadlineTimeDistance = -1;

    for (int i = 0; i < processVector.size(); i++){
      if(i == currentProcess){
        //System.out.println("Continue ");
        continue;
      }
      process = (sProcess) processVector.elementAt(i);
      //System.out.println("Find process: " + i + " data " + process.cputime + " " + process.ioblocking + " " + process.ionext + " " + process.cpudone);

      if(process.cpudone < process.cputime && process.ioblocking - process.ionext > 0){ //process isnot blocked
        if(process.ioblocking - process.ionext < deadlineTimeDistance || deadlineTimeDistance < 0){
          nextProcessIndex = i;
          deadlineTimeDistance = process.ioblocking - process.ionext;
        }
      }
    }
    if(nextProcessIndex<0){
      //System.out.println("Less ");
      nextProcessIndex = 0;
    }
    return nextProcessIndex;
  }
}
