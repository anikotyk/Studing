# https://leetcode.com/problems/implement-strstr/
class Solution:
    def strStr(self, haystack: str, needle: str) -> int:
        res=-1
        if(haystack==""):
            if(haystack==needle):
                return 0
            else:
                return res
        for i in range(0, len(haystack)):
            if(haystack[i:i+len(needle)]==needle):
                res=i
                break
        return res
