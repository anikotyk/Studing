#Регіональний пошук. Метод 2-d дерева

import matplotlib.pyplot as plt

class Node:
	def __init__(self, point):
		self.point = point
		self.left = None
		self.right = None

def build(points, depth):
        if (len(points) == 0):
                return None
        if (len(points) == 1):
                return Node(points[0])

        axis = depth % 2

        points = sorted(points, key=lambda x: x[axis])

        centralIndex = len(points) // 2

        root = Node(points[centralIndex])

        root.left = build(points[0:centralIndex], depth + 1)
        root.right = build(points[centralIndex+1::], depth + 1)

        return root

def isInAxisRegion(point, region, axis):
        if(point[axis]>=region[axis][0] and point[axis]<=region[axis][1]):
                return True
        return False

def regionalSearch(root, region, depth, result = []):
        if not root:
                return result

        axis = depth % 2

        if(isInAxisRegion(root.point, region, axis)):
                result = regionalSearch(root.right, region, depth + 1, result)   
                result = regionalSearch(root.left, region, depth + 1, result)

                if(isInAxisRegion(root.point, region, (axis+1) % 2)):
                        result.append(root.point)
        else:
                if(root.point[axis] < region[axis][0]):
                        result = regionalSearch(root.right, region, depth + 1, result)    
                if(root.point[axis] > region[axis][1]):
                        result = regionalSearch(root.left, region, depth + 1, result)

        return result
    

points = [[5,10],[5,5],[15,15],[3, 6], [0,2], [17, 15], [10,10], [13, 15], [6, 12], [9, 1], [2, 7], [10, 19]]
region = [[1, 20], [1, 10]]

root = build(points, 0)

result = regionalSearch(root, region, 0)

fig, ax = plt.subplots(figsize=(8,8))
for point in points:
        if point in result:
                ax.scatter(*point, color='blue')
        else:
                ax.scatter(*point, color='red')


regionPoints = [(region[0][0], region[1][0]), (region[0][1], region[1][0]), (region[0][1], region[1][1]), (region[0][0], region[1][1])]

x, y = zip(*(regionPoints + [regionPoints[0]]))
plt.plot(x, y)

plt.show()
