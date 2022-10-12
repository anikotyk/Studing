package main

import (
	"fmt"
	"math/rand"
	"sync"
)

func main() {
	arrays := createArrays(3, 5)
	waitGroup := new(sync.WaitGroup)

	startWork(arrays, waitGroup)
}

func createArrays(count, size int) [][]int {
	arrays := make([][]int, count, size)

	for i := 0; i < count; i++ {
		arrays[i] = make([]int, size)

		for j := 0; j < size; j++ {
			arrays[i][j] = rand.Intn(10)
		}
	}

	return arrays
}

func startWork(arrays [][]int, waitGroup *sync.WaitGroup) {
	for {
		waitGroup.Add(len(arrays))

		for i := 0; i < len(arrays); i++ {
			go changeArray(arrays, i, waitGroup)
		}
		waitGroup.Wait()

		showArrays(arrays)
		if compareSumOfArrays(arrays) {

			fmt.Println("Sums of all arrays are equal")
			break
		}
		fmt.Println("Sums are not equal. Continue changing")
	}
}

func changeArray(arrays [][]int, indexArray int, waitGroup *sync.WaitGroup) {
	index := rand.Intn(len(arrays[indexArray]))

	toAdd := 1
	if rand.Intn(10)%2 == 0 {
		toAdd = -1
	}

	arrays[indexArray][index] += toAdd

	waitGroup.Done()
}

func compareSumOfArrays(arrays [][]int) bool {
	sum := -1

	for i := 0; i < len(arrays); i++ {
		sumTmp := arraySum(arrays[i])
		if sum < 0 {
			sum = sumTmp
		} else if sum != sumTmp {
			return false
		}
	}

	return true
}

func arraySum(array []int) int {
	sum := 0
	for i := 0; i < len(array); i++ {
		sum += array[i]
	}

	return sum
}

func showArrays(arrays [][]int) {
	for i := 0; i < len(arrays); i++ {
		fmt.Println(arrays[i])
	}
	fmt.Println()
}
