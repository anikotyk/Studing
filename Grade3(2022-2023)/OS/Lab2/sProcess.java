public class sProcess {
  public int cputime;
  public int timebeforeblocking;
  public int cpudone;
  public int timebeforeblockingdone;
  public int blockingtime;
  public int blockingtimedone;
  public int numblocked;

  public sProcess (int cputime, int ioblocking, int cpudone, int ionext, int numblocked, int blockingtime, int blockingtimedone) {
    this.cputime = cputime;
    this.timebeforeblocking = ioblocking;
    this.cpudone = cpudone;
    this.timebeforeblockingdone = ionext;
    this.numblocked = numblocked;
    this.blockingtime = blockingtime;
    this.blockingtimedone = blockingtimedone;
  } 	
}
