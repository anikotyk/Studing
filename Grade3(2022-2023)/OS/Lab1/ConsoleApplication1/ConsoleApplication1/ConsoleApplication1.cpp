#include <iostream>
#include "trialfuncs.hpp"
#include "compfuncs.hpp"
#include <windows.h>
#include <stdio.h>
#include <atlstr.h>

unsigned int WM_GET_CALCULATION_ARGUMENT;

bool IsError = false;
bool IsCancelled = false;
bool IsCoutLocked = false;
int ResultF = -1;
int ResultG = -1;
int CancellationTiming = 1000;
int MaxCalculationTime = 10000;

PROCESS_INFORMATION ProcessF;
PROCESS_INFORMATION ProcessG;

PROCESS_INFORMATION CreateProcess(LPWSTR lpApplicationName)
{
    STARTUPINFO si;
    PROCESS_INFORMATION pi;

    ZeroMemory(&si, sizeof(si));
    si.cb = sizeof(si);
    ZeroMemory(&pi, sizeof(pi));

    if (!CreateProcess(NULL,   // No module name (use command line)
        lpApplicationName,        // Command line
        NULL,           // Process handle not inheritable
        NULL,           // Thread handle not inheritable
        FALSE,          // Set handle inheritance to FALSE
        0,              // No creation flags
        NULL,           // Use parent's environment block
        NULL,           // Use parent's starting directory 
        &si,            // Pointer to STARTUPINFO structure
        &pi)           // Pointer to PROCESS_INFORMATION structure
        )
    {
        IsError = true;
        std::cout << "CreateProcess error: " << GetLastError()<<"\n";
    }

    return pi;
}

int SendDataToProcessAndGetResult(PROCESS_INFORMATION pi, int data)
{
    //Wait for process to create message queue
    WaitForSingleObject(pi.hProcess, 100);
    
    if (!PostThreadMessage(pi.dwThreadId, WM_GET_CALCULATION_ARGUMENT, 0, data))
    {
        IsError = true;
        std::cout << "PostThreadMessage error: " << GetLastError()<<"\n";
    }
    
    WaitForSingleObject(pi.hProcess, INFINITE);
        
    DWORD result;
    GetExitCodeThread(pi.hThread, &result);

    CloseHandle(pi.hProcess);
    CloseHandle(pi.hThread);

    return result;
}

void StopProcess(PROCESS_INFORMATION pi) {
    TerminateProcess(pi.hProcess, 0);
}

void CalculateF(int x) {
    ProcessF = CreateProcess(CA2CT("fFunc.exe"));
    int res = SendDataToProcessAndGetResult(ProcessF, x);
    ResultF = res;
}

void CalculateG(int x) {
    ProcessG = CreateProcess(CA2CT("gFunc.exe"));
    int res = SendDataToProcessAndGetResult(ProcessG, x);
    ResultG = res;
}

void Cancellation() {
    std::string input;
    while (true) {
        Sleep(CancellationTiming);
        IsCoutLocked = true;
        std::cout << "Enter a - to continue calculations, b - to continue calculations without prompt, c - to cancell calculations: ";
        std::cin >> input;
        if (input == "c") {
            IsCancelled = true;
        }
        else if (input == "b") {
            IsCoutLocked = false;
            break;
        }
        IsCoutLocked = false;
    }
}

void CheckForResult() {
    auto timeStart = std::chrono::high_resolution_clock::now();

    while (true) {
        if (IsCoutLocked) {
            continue;
        }

        if (IsError) {
            std::cout << "Something went wrong. Calculations cancelled\n";
            StopProcess(ProcessG);
            StopProcess(ProcessF);
            break;
        }

        if (IsCancelled) {
            std::cout << "User cancelled calculations\n";
            if (ResultF >= 2 && ResultG >= 2) {
                std::cout << "Result of calculations: " << (ResultF - 2) + (ResultG - 2) << "\n";
            }
            StopProcess(ProcessG);
            StopProcess(ProcessF);
            break;
        }

        auto timeCurrent = std::chrono::high_resolution_clock::now();
        double deltaTime = std::chrono::duration<double, std::milli>(timeCurrent - timeStart).count();
        if (deltaTime > MaxCalculationTime) {
            if (ResultF >= 0) {
                std::cout << "Function g didn`t answer longer than "<< MaxCalculationTime<< " ms. Calculations cancelled\n";
            }
            else if (ResultG >= 0) {
                std::cout << "Function f didn`t answer longer than " << MaxCalculationTime << " ms. Calculations cancelled\n";
            }
            else {
                std::cout << "Functions f and g didn`t answer longer than " << MaxCalculationTime << " ms. Calculations cancelled\n";
            }

            StopProcess(ProcessG);
            StopProcess(ProcessF);
            break;
        }

        if (ResultF == 0) {
            std::cout << "Function f finished with hard fail. Calculations cancelled \n";
            StopProcess(ProcessG);
            break;
        }

        if (ResultG == 0) {
            std::cout << "Function g finished with hard fail. Calculations cancelled \n";
            StopProcess(ProcessF);
            break;
        }

        if (ResultF == 1) {
            std::cout << "Function f finished with soft fail. Calculations cancelled \n";
            StopProcess(ProcessG);
            break;
        }

        if (ResultG == 1) {
            std::cout << "Function g finished with soft fail. Calculations cancelled \n";
            StopProcess(ProcessF);
            break;
        }

        if (ResultF != -1 && ResultG != -1) {
            std::cout << "Result of calculations: " << (ResultF-2) + (ResultG-2) << "\n";
            break;
        }
    }

}

void Manager() 
{
    int x;
    std::cout << "Enter x: ";
    std::cin >> x;

    std::thread fComputationThread(CalculateF, x);
    std::thread gComputationThread(CalculateG, x);
    std::thread cancellation(Cancellation);
    std::thread checkForResultThread(CheckForResult);

    checkForResultThread.join();

    if (cancellation.joinable()) {
        cancellation.detach();
    }
    if (fComputationThread.joinable()) {
        fComputationThread.detach();
    }
    if (gComputationThread.joinable()) {
        gComputationThread.detach();
    }
}

int main(int argc, char* argv[]) 
{
    WM_GET_CALCULATION_ARGUMENT = RegisterWindowMessage(L"GetCalculationArgument");

    std::thread managerThread(Manager);
    managerThread.join();
      
	return 0;
}