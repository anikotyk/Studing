# https://leetcode.com/problems/two-sum/
class Solution:
    def twoSum(self, nums, target):
        dict1={}
        for i in range(0, len(nums)):
            if(target-nums[i] in dict1):
                return([i, dict1[target-nums[i]]])
            dict1.update({nums[i]:i})
