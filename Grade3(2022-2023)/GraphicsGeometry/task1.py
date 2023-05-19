#Локалізація точки на планарному розбитті методом ланцюгів

import copy
import math

import matplotlib.pyplot as plt
import numpy


class Vertex:
    def __init__(self, x, y, index) -> None:
        self.inList = None
        self.outList = None

        self.x = x
        self.y = y
    
        self.index = index
        self.weight = 1
        self.neighbours = []

    def __lt__(self, other):
        return self.y < other.y or (self.y == other.y and self.x < other.x)

    def __str__(self):
        return str(f"{self.index}: ({self.x}; {self.y})")

    def createInOutLists(self):
        self.inList = []
        self.outList = []

        for neighbour in self.neighbours:
            if neighbour.index < self.index:
                self.inList.append(neighbour)
            else:
                self.outList.append(neighbour)

        self.inList = sortClockwise(self.inList, self)
        self.inList.reverse()

        self.outList = sortClockwise(self.outList, self)

    def buildPlot(self, ax):
        ax.scatter(self.x, self.y, color="red")
        return ax


class Graph:
    def __init__(self):
        self.vertices = []
        self.weights = None

    def addVertex(self, vertex):
        self.vertices.append(vertex)

    def addEdge(self, first, second):
        self.vertices[first].neighbours.append(self.vertices[second])
        self.vertices[second].neighbours.append(self.vertices[first])

    def initializeWeights(self):
        self.weights = []
        for i, _ in enumerate(self.vertices):
            self.weights.append([-1] * len(self.vertices))

        for vertex in self.vertices:
            for another_vertex in vertex.neighbours:
                self.weights[vertex.index][another_vertex.index] = 1

    def getChains(self):
        self.sortVertices()
        self.balance()
        weights = copy.deepcopy(self.weights)

        chains_count = 0
        for vertex in self.vertices[0].outList:
            chains_count += weights[self.vertices[0].index][vertex.index]

        chains = []
        for _ in range(chains_count):
            chain = []
            current_vertex = self.vertices[0]
            while current_vertex != self.vertices[-1]:
                chain.append(current_vertex)
                j = 0
                while weights[current_vertex.index][current_vertex.outList[j].index] < 1:
                    j += 1

                weights[current_vertex.index][current_vertex.outList[j].index] -= 1
                weights[current_vertex.outList[j].index][current_vertex.index] -= 1
                current_vertex = current_vertex.outList[j]

            chain.append(self.vertices[-1])

            chains.append(chain)

        return chains

    def sortVertices(self):
        self.vertices.sort()
        for i, vertex in enumerate(self.vertices):
            vertex.index = i

    def buildPlot(self):
        xs = []
        ys = []
        labels = []

        for vertex in self.vertices:
            xs.append(vertex.x)
            ys.append(vertex.y)
            labels.append(str(vertex.index))

        fig, ax = plt.subplots(nrows=1, ncols=1, figsize=(10, 10))

        ax.scatter(xs, ys, color="green")

        ax.set_xlim([min(xs) - 1, max(xs) + 1])
        ax.set_ylim([min(ys) - 1, max(ys) + 1])

        for vertex in self.vertices:
            for neighbour in vertex.neighbours:
                ax.plot([vertex.x, neighbour.x], [vertex.y, neighbour.y], "blue")

        for i in range(len(xs)):
            ax.annotate(labels[i], (xs[i], ys[i]), xytext=(xs[i] - 0.025, ys[i] + 0.1))

        ax.set_ylabel("y", rotation=0, labelpad=20)
        ax.set_xlabel("x", rotation=0, labelpad=20)

        return fig, ax

    def balance(self):
        self.initializeWeights()

        self.createInOutLists()

        for index in range(1, len(self.vertices) - 1):
            current_vertex = self.vertices[index]
            leftest_vertex = current_vertex.outList[0]
            w_in = 0
            for vertex in current_vertex.inList:
                w_in += self.weights[vertex.index][current_vertex.index]

            w_out = 0
            for vertex in current_vertex.outList:
                w_out += self.weights[current_vertex.index][vertex.index]

            if w_in > w_out:
                self.weights[current_vertex.index][leftest_vertex.index] += w_in - w_out
                self.weights[leftest_vertex.index][current_vertex.index] = self.weights[current_vertex.index][leftest_vertex.index]

        for index in range(len(self.vertices) - 1, 0, -1):
            current_vertex = self.vertices[index]
            leftest_vertex = current_vertex.inList[0]
            w_in = 0
            for vertex in current_vertex.inList:
                w_in += self.weights[vertex.index][current_vertex.index]

            w_out = 0
            for vertex in current_vertex.outList:
                w_out += self.weights[current_vertex.index][vertex.index]

            if w_out > w_in:
                self.weights[leftest_vertex.index][current_vertex.index] += w_out - w_in
                self.weights[current_vertex.index][leftest_vertex.index] = self.weights[leftest_vertex.index][current_vertex.index]

    def createInOutLists(self):
        for vertex in self.vertices:
            vertex.createInOutLists()
    
def getPointLocationVertices(chain1, chain2, point):
    end = max(len(chain1), len(chain2))
    chain1Index = 0
    chain2Index = 0
    subgraphes = [[]]
    i = 0
    while(chain1Index < end and chain2Index < end):
        if(chain1[chain1Index].index == chain2[chain2Index].index):
            if (len(subgraphes)>0):
                subgraphes[-1].append(chain1[chain1Index])

            subgraphes.append([])
            subgraphes[-1].append(chain1[chain1Index])
            chain1Index+=1
            chain2Index+=1
        else:
            while(chain1[chain1Index].index < chain2[chain2Index].index):
                subgraphes[-1].append(chain1[chain1Index])
                chain1Index+=1
            while(chain2[chain2Index].index < chain1[chain1Index].index):
                subgraphes[-1].append(chain2[chain2Index])
                chain2Index+=1
    result = []
    for subgraph in subgraphes:
        if(isPointInSubgraph(subgraph, point)):
            for vertice in subgraph:
                result.append(vertice.index)
    return result

def isPointInSubgraph(subgraph, point):
    n = len(subgraph)
    inside = False

    p1x, p1y = subgraph[0].x, subgraph[0].y
    for i in range(n + 1):
        p2x, p2y = subgraph[i % n].x, subgraph[i % n].y

        if point.y > min(p1y, p2y):
            if point.y <= max(p1y, p2y):
                if point.x <= max(p1x, p2x):
                    if p1y != p2y:
                        x_intersect = (point.y - p1y) * (p2x - p1x) / (p2y - p1y) + p1x
                    if p1x == p2x or point.x <= x_intersect:
                        inside = not inside

        p1x, p1y = p2x, p2y

    return inside
            

def localizePoint(currentChains, point):
    isOnLeft = False
    center = int(len(currentChains) / 2 - 1)

    if len(currentChains) > 1:
        isOnLeft = discriminatePoint(currentChains[center], point)

        if not isOnLeft and discriminatePoint(currentChains[center + 1], point):
            return [currentChains[center], currentChains[center + 1]]

    if len(currentChains) == 1:
        if discriminatePoint(currentChains[0], point):
            return [None, currentChains[0]]
        else:
            return [currentChains[0], None]

    if isOnLeft:
        return localizePoint(currentChains[:center + 1], point)
    else:
        return localizePoint(currentChains[center + 1:], point)


def discriminatePoint(chain, point):
    if len(chain) == 2:
        return isPointOnLeft(chain[0], chain[1], point)
    length = len(chain)
    center = int(length / 2)
    if point.y < chain[center].y:
        return discriminatePoint(chain[:center + 1], point)
    else:
        return discriminatePoint(chain[center:], point)


def isPointOnLeft(chainPoint1, chainPoint2, point):
    return ((chainPoint2.x - chainPoint1.x) * (point.y - chainPoint1.y) - (chainPoint2.y - chainPoint1.y) * (
            point.x - chainPoint1.x)) >= 0


def sortClockwise(vertices: list, center_vertex):
    return sorted(vertices, key=lambda vertex: math.atan2(vertex.y - center_vertex.y,
                                                          vertex.x - center_vertex.x), reverse=True)

def getData(path):
    file = open(path, "r")
    
    nVertices = int(file.readline())
    nEdges = int(file.readline())

    graph = Graph()

    for i in range(nVertices):
        line = str(file.readline())
        coordinates = line.split(sep=" ")
        graph.addVertex(Vertex(float(coordinates[0]), float(coordinates[1]), i))

    for i in range(nEdges):
        line = str(file.readline())
        vertices = line.split(sep=" ")
        graph.addEdge(int(vertices[0]), int(vertices[1]))

    return graph


def buildPlotForChain(chain, ax, color, offset):
    for i in range(1, len(chain)):
        ax.plot([chain[i - 1].x + offset, chain[i].x + offset], [chain[i - 1].y + offset, chain[i].y + offset], color=color)

    return ax

graph = getData("input.txt")
point = Vertex(6, 5, -1)

chains = graph.getChains()

pointLocationChains = localizePoint(chains, point)
pointLocationChains = list(filter(lambda item: item is not None, pointLocationChains))

figure, axes = graph.buildPlot()

if(len(pointLocationChains)>1):
    print("\nThe point is located between chains: ")
    for chain in pointLocationChains:
        if chain is not None:
            #axes = buildPlotForChain(chain, axes, "red", 0)
            
            print("Chain: ", end = "")
            for i in range(0, len(chain)):
                print(chain[i].index, end=" ")
            print("")
    res = getPointLocationVertices(pointLocationChains[0], pointLocationChains[1], point)
    print("\nAnswer: ")
    for i in res:
        print(i, end=" ")
else:
    print("\nThe point is outside of graph")
axes = point.buildPlot(axes)
plt.show()
