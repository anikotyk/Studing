package main

import (
	"fmt"
	"math/rand"
	"time"
)

func main() {
	tutun := make(chan *SmokingParts)
	paper := make(chan *SmokingParts)
	fire := make(chan *SmokingParts)

	semaphore := make(chan bool, 1)
	main := []chan *SmokingParts{tutun, paper, fire}

	go setSmokingParts(main, semaphore)
	go smoke("Tutun", tutun, semaphore)
	go smoke("Papper", paper, semaphore)
	go smoke("Fire", fire, semaphore)

	for {
		time.Sleep(10000 * time.Millisecond)
	}
}

type SmokingParts struct {
	smokingParts [3]bool
}

func smoke(name string, mainChan chan *SmokingParts, semaphore chan bool) {
	for {
		<-mainChan
		fmt.Println("Now smoking owner of " + name)
		<-semaphore
	}
}

func setSmokingParts(chanArray []chan *SmokingParts, semaphore chan bool) {
	for {
		semaphore <- true

		array := [3]bool{true, true, true}
		index := rand.Int() % len(array)
		array[index] = false
		componentsText := ""
		if array[0] {
			componentsText += "tutun "
		}
		if array[1] {
			componentsText += "papper "
		}
		if array[2] {
			componentsText += "fire "
		}
		fmt.Println("Get smoking parts " + componentsText)
		chanArray[index] <- &SmokingParts{array}
	}
}
