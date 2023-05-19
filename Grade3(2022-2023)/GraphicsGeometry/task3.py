#Найближча пара множини точок на площині – метод «розділяй і владарюй».

import math
import matplotlib.pyplot as plt
 
class Point:
        def __init__(self, x, y):
                self.x = x
                self.y = y

def dist(p1, p2):
        return math.sqrt((p1.x - p2.x)*(p1.x - p2.x) + (p1.y - p2.y)*(p1.y - p2.y))
 
def findClosestPointsBruteForce(points):
        minDist = float("inf")
        closestPoints = []
        for i in range(0, len(points)):
                for j in range(i+1, len(points)):
                        currentDist = dist(points[i], points[j])
                        if (currentDist < minDist):
                                closestPoints = [points[i], points[j]]
                                minDist = currentDist
        return closestPoints

def findClosestPointsStrip(strip):
        minDist = float("inf")
        closestPoints = []
        
        strip = sorted(strip, key=lambda point: point.y)

        for i in range(0, len(strip)):
                for j in range(i+1, len(strip)):
                        if (strip[j].y - strip[i].y) >= minDist:
                                break
                        
                        currentDist = dist(strip[i], strip[j])
                        if (currentDist < minDist):
                                closestPoints = [strip[i], strip[j]]
                                minDist = currentDist
                
        return closestPoints
 
def findClosestPointsRecursive(points, n):
        if n <= 3:
                return findClosestPointsBruteForce(points)

        center = n//2
        centralPoint = points[center]
        
        leftClosestPoints = findClosestPointsRecursive(points, center)
        minDistLeft = dist(leftClosestPoints[0], leftClosestPoints[1])
        
        rightClosestPoints = findClosestPointsRecursive(points[center:], n - center)
        minDistRight = dist(rightClosestPoints[0], rightClosestPoints[1])
        
        if(minDistLeft < minDistRight):
                minDist = minDistLeft
                closestPoints = leftClosestPoints
        else:
                minDist = minDistRight
                closestPoints = rightClosestPoints

        strip = []
        for i in range(0, n):
                if (abs(points[i].x - centralPoint.x) < minDist):
                        strip.append(points[i])

        minDistStrip = float("inf") 
        if(len(strip)>1):
                stripClosestPoints = findClosestPointsStrip(strip)
                minDistStrip = dist(stripClosestPoints[0], stripClosestPoints[1])

        if(minDistStrip <= minDist):
                closestPoints = stripClosestPoints

        return closestPoints
 
 
def findClosestPair(points):
        points = sorted(points, key=lambda point: point.x)
        return findClosestPointsRecursive(points, len(points))

points = [Point(8, 10.1),Point(6, 6),Point(8, 1),Point(1, 3), Point(2, 16), Point(15, 7), Point(7, 1), Point(8, 10), Point(9, 2), Point(11, 11), Point(5, 4)]

closestPoints = findClosestPair(points)
minDist = dist(closestPoints[0], closestPoints[1])

fig, ax = plt.subplots(figsize=(8,8))
for point in points:
        if point in closestPoints:
                ax.scatter(*[point.x, point.y], color='red')
        else:
                ax.scatter(*[point.x, point.y], color='blue')
                
closestPointsX = (closestPoints[0].x, closestPoints[1].x)
closestPointsY = (closestPoints[0].y, closestPoints[1].y)
plt.plot(closestPointsX, closestPointsY, color = "red")

centerX = sum(closestPointsX)/2
centerY = sum(closestPointsY)/2
plt.text(centerX, centerY + 1, "%.2f" % minDist)

plt.show()
