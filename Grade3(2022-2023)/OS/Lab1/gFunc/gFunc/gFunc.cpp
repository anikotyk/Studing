#include <iostream>
#include <windows.h>
#include "trialfuncs.hpp"

unsigned int WM_GET_CALCULATION_ARGUMENT;
int MaxAttemptsCount = 3;

int Calculate(int x, int attemptNumber) {
    auto result = os::lab1::compfuncs::trial_g<os::lab1::compfuncs::INT_SUM>(x);

    if (std::holds_alternative<int>(result)) {
        return std::get<int>(result) + 2;
    }
    else if (std::holds_alternative<os::lab1::compfuncs::soft_fail>(result)) {
        if (attemptNumber < MaxAttemptsCount) {
            return Calculate(x, attemptNumber + 1);
        }
        else {
            //soft fail code 1
            return 1;
        }
    }
    else {
        //hard fail code 0
        return 0;
    }
}

int main()
{
    MSG msg;
    WM_GET_CALCULATION_ARGUMENT = RegisterWindowMessage(L"GetCalculationArgument");

    while (GetMessage(&msg, NULL, 0, 0))
    {
        if (msg.message == WM_GET_CALCULATION_ARGUMENT)
        {
            return Calculate(msg.lParam, 1);
        }

        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    return 0;
}