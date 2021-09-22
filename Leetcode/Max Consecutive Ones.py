class Solution:
    def findMaxConsecutiveOnes(self, nums: List[int]) -> int:
        maxl=0
        cur=0
        for i in nums:
            if(i==1):
                cur+=1
            else:
                if(cur>maxl):
                    maxl=cur
                cur=0
        if(cur>maxl):
            maxl=cur        
        return maxl
