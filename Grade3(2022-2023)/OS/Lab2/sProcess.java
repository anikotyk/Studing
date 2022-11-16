public class sProcess {
  public int cputime;
  public int cpudone;
  public int period;
  public int deadline;
  public int processingtime;
  public int processingtimedone;


  public sProcess (int cputime, int cpudone, int period, int deadline, int processingtime, int processingtimedone) {
    this.cputime = cputime;
    this.cpudone = cpudone;
    this.period = period;
    this.deadline = deadline;
    this.processingtime = processingtime;
    this.processingtimedone = processingtimedone;
  } 	
}
