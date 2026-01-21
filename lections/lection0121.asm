section .data
    x dq 2.2
    y dq 3.3
    z dq 0.0

section .text
global main
main:
    mov ebp, esp; for correct debugging
    finit
    fld1
    fld qword [x] ; ST(0) =2.2
    fld qword [y] ; ST(0) = 3.3, ST(1) = 2.2
    fxch st1
    fadd
    fadd st2, st0
    faddp st2, st0
    fsub
    fsub st0, st2
    fsub st2, st0
    fmul st0, st1
    fdiv st0, st1
    fst qword [z]
    
    xor eax, eax
    ret