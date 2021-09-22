# https://leetcode.com/problems/random-pick-index/
class Solution:
    arr=[]
    def __init__(self, nums: List[int]):
        self.arr=nums

    def pick(self, target: int) -> int:
        index=[]
        
        for i in range(0, len(self.arr)):
            if(self.arr[i]==target):
                index.append(i)
        return index[randrange(len(index))]


# Your Solution object will be instantiated and called as such:
# obj = Solution(nums)
# param_1 = obj.pick(target)
