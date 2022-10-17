package main

import (
	"fmt"
	"strconv"
	"sync"
	"time"
)

func main() {
	countBusStations := 3
	maxCountBusesOnStation := 2
	busesCount := 10
	StartBuses(busesCount, maxCountBusesOnStation, countBusStations)
}

type Bus struct {
	indexCurrentBusStation int
	number                 int
}

func StartBuses(busesCount, maxCountBusesOnStation, countBusStations int) {
	waitGroup := &sync.WaitGroup{}

	busStations := make([]chan struct{}, countBusStations)
	for i := 0; i < countBusStations; i++ {
		busStations[i] = make(chan struct{}, maxCountBusesOnStation)
	}

	buses := make([]Bus, busesCount)
	for i := 0; i < busesCount; i++ {
		buses[i] = Bus{-1, i}
	}

	for {
		waitGroup.Add(busesCount)
		for i := 0; i < busesCount; i++ {
			buses[i].indexCurrentBusStation += 1
			go EnterBusStation(buses[i], busStations[buses[i].indexCurrentBusStation], waitGroup)
		}

		waitGroup.Wait()
	}
}

func EnterBusStation(bus Bus, busStation chan struct{}, waitGroup *sync.WaitGroup) {
	busStation <- struct{}{}

	fmt.Println("Bus " + strconv.Itoa(bus.number) + " is on station " + strconv.Itoa(bus.indexCurrentBusStation))
	time.Sleep(time.Millisecond * 1000)
	fmt.Println("Bus " + strconv.Itoa(bus.number) + " exits station " + strconv.Itoa(bus.indexCurrentBusStation))
	waitGroup.Done()

	<-busStation
}
