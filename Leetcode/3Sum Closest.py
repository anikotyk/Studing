# https://leetcode.com/problems/3sum-closest/
import math
class Solution:
    def threeSumClosest(self, nums: List[int], target: int) -> int:
        res=math.inf
        for i in range(0, len(nums)-2):
            for j in range(i+1, len(nums)-1):
                for k in range(j+1, len(nums)):
                    if (abs(sum([nums[i], nums[j], nums[k]])-target)<abs(res-target)):
                        res=sum([nums[i], nums[j], nums[k]])
                        if (res-target==0):
                            return res
        return res
