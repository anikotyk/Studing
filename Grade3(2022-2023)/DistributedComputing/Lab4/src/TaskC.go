package main

import (
	"fmt"
	"strconv"
	"sync"
)

const Duration = 10000

func main() {
	graph := Graph{}
	graph.CreateGraphList()

	go CityController(&graph, "London", true)
	go CityController(&graph, "Boston", true)
	go RouteController(&graph, "London", "Boston", true, 10)
	go CityController(&graph, "Paris", true)
	go RouteController(&graph, "London", "Boston", true, 10)
	go FindRouteController(&graph, "London", "Boston")
	go PriceController(&graph, "London", "Boston", 150)
	go FindRouteController(&graph, "London", "Boston")
	go CityController(&graph, "Paris", false)

	for {
	}
}

func PriceController(graph *Graph, startCityName string, endCityName string, price int) {
	if graph.routesArray[startCityName] == nil {
		fmt.Println("Price controller failed " + startCityName + " isn`t in graph")
		return
	}
	if graph.routesArray[endCityName] == nil {
		fmt.Println("Price controller failed " + endCityName + " isn`t in graph")
		return
	}

	route, _ := graph.GetRoute(startCityName, endCityName)
	if route == nil {
		fmt.Println("No route between " + startCityName + " and " + endCityName)
	} else {
		graph.ChangeRoutePrice(startCityName, endCityName, price)
		fmt.Println("New price between " + startCityName + " and " + endCityName + " is " + strconv.Itoa(price))
	}
}

func RouteController(graph *Graph, startCityName string, endCityName string, isToAdd bool, price int) {
	if graph.routesArray[startCityName] == nil {
		fmt.Println("Route controller failed " + startCityName + " isn`t in graph")
		return
	}
	if graph.routesArray[endCityName] == nil {
		fmt.Println("Route controller failed " + endCityName + " isn`t in graph")
		return
	}

	if isToAdd {
		graph.AddRoute(startCityName, endCityName, price)
		fmt.Println("Added route between " + startCityName + " and " + endCityName + " with price " + strconv.Itoa(price))
	} else {
		graph.DeleteRoute(startCityName, endCityName)
		fmt.Println("Deleted route between " + startCityName + " and " + endCityName)
	}
}

func CityController(graph *Graph, cityName string, isToAdd bool) {
	if isToAdd {
		graph.AddCity(cityName)
		fmt.Println("Added city " + cityName)
	} else {
		graph.DeleteCity(cityName)
		fmt.Println("Deleted city " + cityName)
	}
}

func FindRouteController(graph *Graph, startCityName string, endCityName string) {
	route, _ := graph.GetRoute(startCityName, endCityName)

	if route != nil {
		fmt.Println("Found route between " + startCityName + " and " + endCityName + " the price is " + strconv.Itoa(route.price))
	} else {
		fmt.Println("No route between " + startCityName + " and " + endCityName)
	}
}

type Graph struct {
	routesArray map[string][]*Route
	locker      sync.RWMutex
}

type Route struct {
	endCity string
	price   int
}

func (graph *Graph) CreateGraphList() {
	graph.routesArray = make(map[string][]*Route)
}

func (graph *Graph) GetRoute(startCityName string, endCityName string) (*Route, int) {
	if graph.routesArray[startCityName] == nil || graph.routesArray[endCityName] == nil {
		return nil, -1
	}

	for i := 0; i < len(graph.routesArray[startCityName]); i++ {
		if graph.routesArray[startCityName][i].endCity == endCityName {
			return graph.routesArray[startCityName][i], i
		}
	}

	return nil, -1
}

func (graph *Graph) AddRoute(startCityName string, endCityName string, price int) {
	if (startCityName == endCityName) || graph.routesArray[startCityName] == nil || graph.routesArray[endCityName] == nil {
		return
	}

	_, routeIndex := graph.GetRoute(startCityName, endCityName)

	if routeIndex != -1 {
		return
	}

	graph.routesArray[startCityName] = append(graph.routesArray[startCityName], &Route{endCityName, price})
	graph.routesArray[endCityName] = append(graph.routesArray[endCityName], &Route{startCityName, price})
}

func (graph *Graph) AddCity(cityName string) {
	if graph.routesArray[cityName] != nil {
		return
	}

	graph.routesArray[cityName] = make([]*Route, 0)
}

func DeleteByIndex[T any](slice []T, i int) []T {
	return append(slice[:i], slice[i+1:]...)
}

func (graph *Graph) DeleteCity(cityName string) {
	if graph.routesArray[cityName] == nil {
		return
	}

	for city := range graph.routesArray {
		if city != cityName {
			graph.DeleteRoute(city, cityName)
		}
	}

	delete(graph.routesArray, cityName)
}

func (graph *Graph) DeleteRoute(startCityName string, endCityName string) {
	_, index := graph.GetRoute(startCityName, endCityName)

	if index == -1 {
		return
	}

	_, index2 := graph.GetRoute(endCityName, startCityName)

	graph.routesArray[startCityName] = DeleteByIndex(graph.routesArray[startCityName], index)
	graph.routesArray[endCityName] = DeleteByIndex(graph.routesArray[endCityName], index2)
}

func (graph *Graph) ChangeRoutePrice(from, to string, price int) {
	route1, _ := graph.GetRoute(from, to)

	if route1 == nil {
		return
	}

	route2, _ := graph.GetRoute(to, from)

	route1.price = price
	route2.price = price
}
