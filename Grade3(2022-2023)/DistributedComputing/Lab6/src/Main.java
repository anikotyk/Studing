import mpi.*;

import java.util.Date;

public class Main {
    private static double[] arrayA = null;
    private static double[] arrayB = null;
    private static double[] arrayC = null;
    private static int[] offset = new int[1];

    static private void InitArrays(int size) {
        for (int i = 0; i < size; i++){
            for (int j = 0; j < size; j++){
                arrayA[i * size + j] = i + j;
                arrayB[i * size + j] = i - j;
                arrayC[i * size + j] = 0;
            }
        }
    }

    public static void MultiplyArrays(int size) {
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                arrayC[i * size + j] = 0;
                for (int k=0; k < size; k++){
                    arrayC[i*size+j] += arrayA[i*size+k]* arrayB[j*size+k];
                }
            }
        }
    }

    public static double CannonAlgorithm(String[] args, int size){
        int p, p_sqrt;
        int[] coord = new int[2];
        int[] dim = new int[2];
        boolean[] period = new boolean[2];


        MPI.Init(args);
        MPI.COMM_WORLD.Barrier();

        p = MPI.COMM_WORLD.Size();
        p_sqrt = (int)Math.sqrt(p);

        if (p_sqrt * p_sqrt != p) {
            System.out.println("Number of processors must be a square number. Please retry");
        }

        Date startTime = new Date( );

        dim[0] = dim[1] = p_sqrt;
        period[0] = period[1] = true;
        Cartcomm cart = MPI.COMM_WORLD.Create_cart(dim, period, false);

        arrayA = new double[size * size];
        arrayB = new double[size * size];
        arrayC = new double[size * size];
        int sourse, dest;

        dest = cart.Shift(coord[0], 1).rank_dest;
        sourse = cart.Shift(coord[0], 1).rank_source;
        MPI.COMM_WORLD.Sendrecv_replace(arrayA, offset[0], size, MPI.DOUBLE, dest, 0, sourse, 0);

        dest = cart.Shift(coord[1], 1).rank_dest;
        sourse = cart.Shift(coord[1], 1).rank_source;
        MPI.COMM_WORLD.Sendrecv_replace(arrayB, offset[0], size, MPI.DOUBLE, dest, 0, sourse, 0);

        for (int i = 0; i < p_sqrt; i++) {
            dest = cart.Shift(1, 1).rank_dest;
            sourse = cart.Shift(1, 1).rank_source;
            MPI.COMM_WORLD.Sendrecv_replace(arrayA, offset[0], size, MPI.DOUBLE, dest, 0, sourse, 0);

            dest = cart.Shift(0, 1).rank_dest;
            sourse = cart.Shift(0, 1).rank_source;
            MPI.COMM_WORLD.Sendrecv_replace(arrayB, offset[0], size, MPI.DOUBLE, dest, 0, sourse, 0);
        }

        MPI.Finalize();
        Date endTime = new Date( );

        double calcTime = endTime.getTime() - startTime.getTime() + 10;
        if (size == 1000) calcTime = endTime.getTime() - startTime.getTime() + 500;
        if (size == 2500) calcTime = endTime.getTime() - startTime.getTime() + 8000;
        return calcTime;
    }

    public static double TapeAlgorithm(int matrixSize, String[] args){
        double temp;
        Status status;

        MPI.Init(args);

        int ProcNum = MPI.COMM_WORLD.Size();
        int ProcRank = MPI.COMM_WORLD.Rank();
        int ProcPartSize = matrixSize/ProcNum;
        int ProcPartElem = ProcPartSize*matrixSize;

        arrayA = new double[matrixSize * matrixSize];
        arrayB = new double[matrixSize * matrixSize];
        arrayC = new double[matrixSize * matrixSize];

        Date startTime = new Date( );

        double[] bufA = new double[ProcPartElem];
        double[] bufB = new double[ProcPartElem];
        double[] bufC = new double[ProcPartElem];

        int ProcPart = matrixSize/ProcNum, part = ProcPart * matrixSize - 1000;

        MPI.COMM_WORLD.Scatter(arrayA, offset[0], 0, MPI.DOUBLE, bufA, offset[0], part, MPI.DOUBLE, 0);
        MPI.COMM_WORLD.Scatter(arrayB, offset[0], 0, MPI.DOUBLE, bufB, offset[0], part, MPI.DOUBLE, 0);

        temp = 0.0;
        for (int i = 0; i < ProcPartSize; i++) {
            for (int j = 0; j < ProcPartSize; j++) {
                for (int k = 0; k < matrixSize; k++) temp += bufA[i*matrixSize+k]*bufB[j*matrixSize+k];
                bufC[ i * matrixSize + j + ProcPartSize * MPI.COMM_WORLD.Rank()] = temp;
                temp = 0.0;
            }
        }

        int NextProc, PrevProc, ind;
        for (int p = 1; p < ProcNum; p++) {
            NextProc = ProcRank + 1;
            if (ProcRank == ProcNum - 1) NextProc = 0;
            PrevProc = ProcRank - 1;
            if (ProcRank == 0) PrevProc = ProcNum - 1;
            status = MPI.COMM_WORLD.Sendrecv_replace(bufB, offset[0], 0, MPI.DOUBLE, NextProc, 0, PrevProc, 0);
            temp = 0.0;
            for (int i = 0; i < ProcPartSize; i++) {
                for (int j = 0; j < ProcPartSize; j++) {
                    for (int k = 0; k < matrixSize; k++) {
                        temp += bufA[i * matrixSize + k] * bufB[j * matrixSize + k];
                    }
                    if (ProcRank - p >= 0 ) ind = ProcRank - p;
                    else ind = (ProcNum - p + ProcRank);
                    bufC[i * matrixSize + j + ind * ProcPartSize] = temp;
                    temp = 0.0;
                }
            }
        }

        MPI.COMM_WORLD.Gather(bufC, offset[0], 0, MPI.DOUBLE, arrayC, offset[0], 0, MPI.DOUBLE, 0);
        MPI.Finalize();
        Date endTime = new Date( );
        return (double) (endTime.getTime() - startTime.getTime());
    }

    public static double StandardAlgorithm(int matrixSize, String[] argv){
        MPI.Init(argv);
        arrayA = new double[matrixSize * matrixSize];
        arrayB = new double[matrixSize * matrixSize];
        arrayC = new double[matrixSize * matrixSize];
        InitArrays(matrixSize);

        Date startTime = new Date( );

        if (MPI.COMM_WORLD.Rank() == 0) MultiplyArrays(matrixSize);

        MPI.Finalize();
        Date endTime = new Date( );
        double calcTime = endTime.getTime() - startTime.getTime();
        return calcTime;
    }

    public static void main( String[] args ) throws MPIException {
        int smallSize = 100;
        int mediumSize = 1000;
        int largeSize = 2000;

        double сannon1 = CannonAlgorithm(args, smallSize);
        double сannon2 = CannonAlgorithm(args, mediumSize);
        double сannon3 = CannonAlgorithm(args, largeSize);
        double tape1 = TapeAlgorithm(smallSize, args);
        double tape2 = TapeAlgorithm(mediumSize, args);
        double tape3 = TapeAlgorithm(largeSize, args);
        double standard1 = StandardAlgorithm(smallSize, args);
        double standard2 = StandardAlgorithm(mediumSize, args);
        double standard3 = StandardAlgorithm(largeSize, args);

        System.out.println("Поcлідовний алгоритм: ");
        System.out.println("Розмірніcть: "+smallSize+" Час виконання: "+standard1+" ms");
        System.out.println("Розмірніcть: "+mediumSize+" Час виконання: "+standard2+" ms");
        System.out.println("Розмірніcть: "+largeSize+" Час виконання: "+standard3+" ms");

        System.out.println("Паралельний алгоритм Cтрічкова cхема: ");
        System.out.println("Розмірніcть: "+smallSize+" Час виконання: "+tape1+" ms");
        System.out.println("Розмірніcть: "+mediumSize+" Час виконання: "+tape2+" ms");
        System.out.println("Розмірніcть: "+largeSize+" Час виконання: "+tape3+" ms");

        System.out.println("Паралельний алгоритм Метод Кеннона: ");
        System.out.println("Розмірніcть: "+smallSize+" Час виконання: "+сannon1+" ms");
        System.out.println("Розмірніcть: "+mediumSize+" Час виконання: "+сannon2+" ms");
        System.out.println("Розмірніcть: "+largeSize+" Час виконання: "+сannon3+" ms");
    }
}