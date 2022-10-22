;;; 231Lib.asm
;;; A simple I/O library for CSC231.
;;; will be expanded as needed.
;;;
;;; D. Thiebaut
;;; Adapted from Swarnali Ahmed's 2002 program: mytools.inc
;;; http://cs.smith.edu/dftwiki
;;;
;;; Contains several functions for performing simple I/O of
;;; data.
;;; _printDec: function that prints an integer on the screen
;;; _printString: function that prints a string on the screen
;;; _println: moves the cursor to the next line on the screen
;;; _getInput: gets a possibly signed integer from the keyboard
;;;
;;; Version 2.  Sept 22, 2014. 
;;;      - updated _getInput to get only 1 char at at time.
;;;      - now works with pipes       
;;; Version 1.  Sept 21, 2014.
;;; 
%assign SYS_EXIT        1
%assign SYS_WRITE       4
%assign STDOUT          1

global	_atoi	
global  _printDec
global  _printInt	
global  _printString
global  _printCString	
global  _println
global  _getInput
global  _printRegs
global  _printHex

        

        section         .text
        
     
;;; ;------------------------------------------------------
;;; ;------------------------------------------------------
;;; ; _atoi:    gets a numerical input in a 0-terminated string
;;; ;	        pointed to by eax.
;;; ;           returns the resulting number in eax (32 bits).
;;; ;           recognizes - as the first character of
;;; ;           negative numbers.  Does not skip whitespace
;;; ;           at the beginning.  Stops on first not decimal
;;; ;           character encountered.
;;; ;
;;; ; NO REGISTERS MODIFIED, except eax
;;; ;
;;; ; Example of call:
;;; ;
;;; ;          call  getInput
;;; ;          mov   dword[x], eax ; put integer in x
;;; ;
;;; ;------------------------------------------------------
;;; ;------------------------------------------------------
_atoi:	
                section .bss
intg2           resd    1
isneg2          resb    1
        
                section .text
                pushad                  ; save all registers

                mov     esi, eax        ; eci --> buffer
                mov     ecx, 0          ; edi = counter of chars
.count:		cmp	byte[esi], 0	; '\0'?
		je	.parse
		inc	esi
		inc	ecx
		jmp	.count
.parse:
                mov     esi, eax        ; esi --> buffer
                mov     ecx, edi        ; loop for all chars received
                mov     dword[intg2], 0
                mov     byte[isneg2], 0

.negativ:      
                cmp     byte[esi], '-'
                jne     .loop
                inc     byte[isneg2]
        
.loop:          mov     ebx, 0
                mov     bl, byte[esi]

                ;; stop on line feed
                cmp     bl, 0		; '\0'?
                je      .done

                ;; stop on non-digit characters
                cmp     bl, '0'
                jb      .done
                cmp     bl, '9'
                ja      .done
        
                ;; bl is a digit...  multiply .int by 10 first
                mov     edx, 10
                mov     eax, dword[intg2]
                mul     edx             ; edx:eax <-- 10 * .int
                
                ;; add int version of char
                sub     bl, '0'
                add     eax, ebx
                mov     dword[intg2], eax
                inc     esi
                loop    .loop
.done:
                ;; if negative, make eax neg
                cmp     byte[isneg2], 0
                je      .return
                neg     eax
                mov     dword [intg2], eax
        
                ;; restore registers and return result in eax
.return:

                popad
                mov     eax, [intg2]
                ret

;;; ;------------------------------------------------------
;;; ;------------------------------------------------------
;;; ; getInput: gets a numerical input from the keyboard.
;;; ;           returns the resulting number in eax (32 bits).
;;; ;           recognizes - as the first character of
;;; ;           negative numbers.  Does not skip whitespace
;;; ;           at the beginning.  Stops on first not decimal
;;; ;           character encountered.
;;; ;
;;; ; NO REGISTERS MODIFIED, except eax
;;; ;
;;; ; Example of call:
;;; ;
;;; ;          call  getInput
;;; ;          mov   dword[x], eax ; put integer in x
;;; ;
;;; ;------------------------------------------------------
;;; ;------------------------------------------------------
_getInput:
                section .bss
buffer          resb    120
intg            resd    1
isneg           resb    1
        
                section .text
                pushad                  ; save all registers

                mov     esi, buffer     ; eci --> buffer
                mov     edi, 0          ; edi = counter of chars
.loop1:        
                mov     eax, 03         ; input
                mov     ebx, 0          ; stdin
                mov     ecx, esi        ; where to put the next char
                mov     edx, 1          ; one char at a time
                int     0x80            ; get the input into buffer

                cmp     byte[esi], 0    ; EOF?
                je      .parse
                cmp     byte[esi], 10   ; line feed?
                je      .parse
                inc     esi             ; point to next cell 
                inc     edi             ; increment char counter
                jmp     .loop1

.parse:
                mov     esi, buffer     ; esi --> buffer
                mov     ecx, edi        ; loop for all chars received
                mov     dword[intg], 0
                mov     byte[isneg], 0

.negativ:      
                cmp     byte[esi], '-'
                jne     .loop
                inc     byte[isneg]
        
.loop:          mov     ebx, 0
                mov     bl, byte[esi]

                ;; stop on line feed
                cmp     bl, 10          ; line feed?
                je      .done

                ;; stop on non-digit characters
                cmp     bl, '0'
                jb      .done
                cmp     bl, '9'
                ja      .done
        
                ;; bl is a digit...  multiply .int by 10 first
                mov     edx, 10
                mov     eax, dword[intg]
                mul     edx             ; edx:eax <-- 10 * .int
                
                ;; add int version of char
                sub     bl, '0'
                add     eax, ebx
                mov     dword[intg], eax
                inc     esi
                loop    .loop
.done:
                ;; if negative, make eax neg
                cmp     byte[isneg], 0
                je      .return
                neg     eax
                mov     dword [intg], eax
        
                ;; restore registers and return result in eax
.return:

                popad
                mov     eax, [intg]
                ret     	
        
        
;;; ;------------------------------------------------------
;;; ;------------------------------------------------------
;;; ; _printDec: takes the double word in eax and prints it
;;; ; to STDOUT in decimal.
;;; ;
;;; ; Examples:
;;; ; print a byte variable
;;; ;          mov     eax, 0
;;; ;          mov     al, byte[someVar]
;;; ;          call    _printDec
;;; ;
;;; ; print a word variable
;;; ;          mov     eax
;;; ;          mov     ax, word[otherVar]
;;; ;          call    _printDec
;;; ;
;;; ; print a double-word variable
;;; ;          mov     eax, dword[thirdVar]
;;; ;          call    _printDec
;;; ;
;;; ; print register edx in decimal
;;; ;
;;; ;          mov     eax, edx
;;; ;          call    _printDec
;;; ;
;;; ;REGISTERS MODIFIED: NONE
;;; ;------------------------------------------------------
;;; ;------------------------------------------------------

_printDec:
;;; saves all the registers so that they are not changed by the function

                section         .bss
.decstr         resb            10
.ct1            resd            1  ; to keep track of the size of the string

                section .text
                pushad                          ; save all registers

                mov             dword[.ct1],0   ; assume initially 0
                mov             edi,.decstr     ; edi points to decstring
                add             edi,9           ; moved to the last element of string
                xor             edx,edx         ; clear edx for 64-bit division
.whileNotZero:
                mov             ebx,10          ; get ready to divide by 10
                div             ebx             ; divide by 10
                add             edx,'0'         ; converts to ascii char
                mov             byte[edi],dl    ; put it in sring
                dec             edi             ; mov to next char in string
                inc             dword[.ct1]     ; increment char counter
                xor             edx,edx         ; clear edx
                cmp             eax,0           ; is remainder of division 0?
                jne             .whileNotZero   ; no, keep on looping

                inc             edi             ; conversion, finish, bring edi
                mov             ecx, edi        ; back to beg of string. make ecx
                mov             edx, [.ct1]     ; point to it, and edx gets # chars
                mov             eax, SYS_WRITE  ; and print!
                mov             ebx, STDOUT
                int             0x80

                popad                           ; restore all registers

                ret
        
;;; ; ------------------------------------------------------------
;;; ; _printString:        prints a string whose address is in
;;; ;                      ecx, and whose total number of chars
;;; ;                      is in edx.
;;; ; Examples:
;;; ; Assume a string labeled msg, containing "Hello World!",
;;; ; and a constant MSGLEN equal to 12.  To print this string:
;;; ;
;;; ;           mov        ecx, msg
;;; ;           mov        edx, MSGLEN
;;; ;           call       _printSTring
;;; ;
;;; ; REGISTERS MODIFIED:  NONE
;;; ; ------------------------------------------------------------

;;; ;save eax and ebx so that it is not modified by the function

_printString:
                push            eax
                push            ebx
        
                mov             eax,SYS_WRITE
                mov             ebx,STDOUT
                int             0x80

                pop             ebx
                pop             eax
                ret

;;; ; ------------------------------------------------------------
;;; ; _printCString:       prints a string whose address is in
;;; ;                      ecx, and which is 0-terminated, like
;;; ;			   C strings.
;;; ; Examples:
;;; ; Assume a string labeled msg, containing "Hello World!",
;;; ; and a constant MSGLEN equal to 12.  To print this string:
;;; ;
;;; ;           mov        ecx, msg     ;there's a \0 somewher in msg
;;; ;           call       _printCString
;;; ;
;;; ; REGISTERS MODIFIED:  NONE
;;; ; ------------------------------------------------------------

;;; ;save eax and ebx so that it is not modified by the function
_printCString:	push		ebx
		push		edx	 	; save regs 
		mov		ebx, ecx 	; make ebx point to string
		mov		edx, 0		; count in edx
.for:		cmp		byte[ebx], 0 	; '\0' found yet?
		je		.done
		inc		ebx 		; no, point to next char 
		inc		edx		; add 1 to counter
		jmp		.for

.done:		call		_printString
		pop		edx
		pop		ebx
		ret
		
;;; ; ------------------------------------------------------------
;;; ; _println             put the cursor on the next line.
;;; ;
;;; ; Example:
;;; ;           call      _println
;;; ;
;;; ; REGISTERS MODIFIED:  NONE
;;; ; ------------------------------------------------------------
_println:  
                section .data
.nl             db              10
        
                section .text
                push            ecx
                push            edx
        
                mov             ecx, .nl
                mov             edx, 1
                call            _printString

                pop             edx
                pop             ecx
                ret

;;; ; ------------------------------------------------------------
;;; ; _printHex: prints contents of eax in hexadecimal, uppercase.
;;; ;            using 8 chars.
;;; ; ------------------------------------------------------------
                section .data
table           db      "0123456789ABCDEF"
hexString       db      "xxxxxxxx"
tempEax         dd      0
        
                section .text
_printHex:
                pushad                  ; save all registers
                mov     [tempEax], eax  ; save eax, as we are going to need it
                                        ; several times, for all its digits 
                mov     ecx, 8          ; get ready to loop 8 times

        ;; get char equivalent to lower nybble of eax
for:            and     eax, 0xF        ; isolate lower nybble of eax
                add     eax, table      ; and translate it into ascii hex char
                mov     bl, byte[eax]   ; bl <- ascii

        ;; make eax point to place where to put this digit in hexString
                mov     eax, hexString  ; beginning of table
                add     eax, ecx        ; add ecx to it, since ecx counts
                dec     eax             ; but eax 1 too big, decrement it
                mov     byte[eax], bl   ; store ascii char at right index in hexString

        ;; shift eax down by 4 bits by dividing it by 16
                mov     ebx, 16         ; ready to shift down by 4 bits
                mov     eax, [tempEax]  ; get eax back
                div     ebx             ; shift down by 4
                mov     [tempEax], eax  ; save again

                loop    for             ; and repeat, 8 times!
        
        ;; print the pattern in hexString, which contains 8 chars

                mov     ecx, hexString  ; 
                mov     edx, 8
                call    _printString

                popad
                ret
          
;;; ; ------------------------------------------------------------
;;; ; _printInt: prints the contents of eax as a 2's complement
;;; ;            number.
;;; ; ------------------------------------------------------------
_printInt:      push    eax			; save the regs we are using
                push    ecx
                push    edx
                
        ;; check if msb is set
                test    eax, 0x80000000
                jz      positive
		neg	eax
                push    eax

		;; the number is negative.  Print a minus sign and
		;; the positive equivalent of the number
                mov     ecx, minus
                mov     edx, 1
                call    _printString
                pop     eax
                
        ;; the number is positive, just print it.
positive:       call    _printDec

                pop     edx
                pop     ecx
                pop     eax
                ret

;;; ; ------------------------------------------------------------
;;; ; printMinus, _printSpace: print a minus sign, and a plus sign.
;;; ; ------------------------------------------------------------
_printMinus:    push    ecx
                push    edx
                mov     ecx, minus
                mov     edx, 1
                call    _printString
                pop     edx
                pop     ecx
                ret

_printSpace:    push    ecx
                push    edx
                mov     ecx, space
                mov     edx, 1
                call    _printString
                pop     edx
                pop     ecx
                ret

;;; ; ------------------------------------------------------------
;;; ; _printRegs: prints all the registers.
;;; ; ------------------------------------------------------------
                section .data
minus           db      '-'
space           db      ' '        
eaxStr          db      'eax '
ebxStr          db      'ebx '
ecxStr          db      'ecx '
edxStr          db      'edx '
esiStr          db      'esi '
ediStr          db      'edi '
eaxTemp         dd      0
ebxTemp         dd      0
ecxTemp         dd      0
edxTemp         dd      0
esiTemp         dd      0
ediTemp         dd      0
                              
                section .text
_printRegs:     mov     [eaxTemp], eax
                mov     [ebxTemp], ebx
                mov     [ecxTemp], ecx
                mov     [edxTemp], edx
                mov     [ediTemp], edi
                mov     [esiTemp], esi

				pushad
				
        ;; print eax
                mov     ecx, eaxStr
                mov     edx, 4
                call    _printString
                call    _printHex
                call    _printSpace
                call    _printDec
                call    _printSpace
                call    _printInt
                call    _println

        ;; print ebx
                mov     ecx, ebxStr
                mov     edx, 4
                call    _printString
                mov		eax, [ebxTemp]
                call    _printHex
                call    _printSpace
                call    _printDec
                call    _printSpace
                call    _printInt
                call    _println

        ;; print ecx
                mov     ecx, ecxStr
                mov     edx, 4
                call    _printString
                mov		eax, [ecxTemp]
                call    _printHex
                call    _printSpace
                call    _printDec
                call    _printSpace
                call    _printInt
                call    _println

        ;; print edx
                mov     ecx, edxStr
                mov     edx, 4
                call    _printString
                mov		eax, [edxTemp]
                call    _printHex
                call    _printSpace
                call    _printDec
                call    _printSpace
                call    _printInt
                call    _println

        ;; print edi
                mov     ecx, ediStr
                mov     edx, 4
                call    _printString
                mov		eax, [ediTemp]
                call    _printHex
                call    _printSpace
                call    _printDec
                call    _printSpace
                call    _printInt
                call    _println

        ;; print esi
                mov     ecx, esiStr
                mov     edx, 4
                call    _printString
                mov		eax, [esiTemp]
                call    _printHex
                call    _printSpace
                call    _printDec
                call    _printSpace
                call    _printInt
                call    _println

		popad 
				
                ret
