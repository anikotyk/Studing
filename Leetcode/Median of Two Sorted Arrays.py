# https://leetcode.com/problems/median-of-two-sorted-arrays/
class Solution:
    def findMedianSortedArrays(self, nums1: List[int], nums2: List[int]) -> float:
        nums=nums1+nums2
        nums.sort()
        if (len(nums)%2==0):
            res=(nums[int(len(nums)/2-1)]+ nums[int(len(nums)/2)])/2
        else:
            res=nums[int(len(nums)/2)]
        return res
