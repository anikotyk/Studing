package main

import (
	"fmt"
	"math/rand"
)

func main() {
	var monks []Monk
	var countMonks = 100

	for i := 0; i < countMonks; i++ {
		if i%2 == 0 {
			monks = append(monks, CreateMonk("Monastery1"))
		} else {
			monks = append(monks, CreateMonk("Monastery2"))
		}

	}

	var ch = make(chan Monk, 1)
	GetWinner(monks, 0, len(monks)-1, ch)
	var winnerMonk = <-ch
	fmt.Println("Monastery " + winnerMonk.monastery + " wins!")
}

type Monk struct {
	monastery string
	energy    int
}

func CreateMonk(monasteryName string) Monk {
	var monk Monk

	monk.energy = rand.Intn(100-1) + 1
	monk.monastery = monasteryName
	return monk
}

func GetWinner(monks []Monk, start, end int, ch chan Monk) {
	var winnerMonk Monk

	if end-start < 2 {
		if monks[start].energy > monks[end].energy {
			winnerMonk = monks[start]
		} else {
			winnerMonk = monks[end]
		}
	} else {
		center := (start + end) / 2
		var ch2 = make(chan Monk, 2)
		go GetWinner(monks, start, center, ch2)
		GetWinner(monks, center+1, end, ch2)
		firstWinner := <-ch2
		secondWinner := <-ch2

		if firstWinner.energy > secondWinner.energy {
			winnerMonk = firstWinner
		} else {
			winnerMonk = secondWinner
		}
	}
	ch <- winnerMonk
}
