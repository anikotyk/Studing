# https://leetcode.com/problems/longest-common-prefix/
class Solution:
    def longestCommonPrefix(self, strs: List[str]) -> str:
        res=''
        flag=False
        if(len(strs)==0):
            return res
        for i in range(0, min([len(s) for s in strs])):
            res+=strs[0][i]
            for j in range(1, len(strs)):
                if(strs[j][i]!=strs[j-1][i]):
                    res=res[:len(res)-1]
                    flag=True
                    break
            if(flag):
                break
        return res
