\ test.fs: ARM dis/assembler testcases

\ Copyright (C) 2009 Free Software Foundation, Inc.

\ This file is part of Gforth.

\ Gforth is free software; you can redistribute it and/or
\ modify it under the terms of the GNU General Public License
\ as published by the Free Software Foundation, either version 3
\ of the License, or (at your option) any later version.

\ This program is distributed in the hope that it will be useful,
\ but WITHOUT ANY WARRANTY; without even the implied warranty of
\ MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
\ GNU General Public License for more details.

\ You should have received a copy of the GNU General Public License
\ along with this program. If not, see http://www.gnu.org/licenses/.

\ Contributed by Andreas Bolka.

." \ --- automatically generated by test.fs ---" cr

also disassembler

\ --

: code:
    code ; immediate
: ;;
    end-code latestxt xt-see ; immediate

\ condition codes

$00000000 dis-CC drop
$10000000 dis-CC drop
$20000000 dis-CC drop
$30000000 dis-CC drop
$40000000 dis-CC drop
$50000000 dis-CC drop
$60000000 dis-CC drop
$70000000 dis-CC drop
$80000000 dis-CC drop
$90000000 dis-CC drop
$A0000000 dis-CC drop
$B0000000 dis-CC drop
$C0000000 dis-CC drop
$D0000000 dis-CC drop
$E0000000 dis-CC drop
$F0000000 dis-CC drop
cr

\ data processing opcodes

code: dp-imm/rot-and r1 42 # r0 and, ;;
code: dp-imm/rot-eor r1 42 # r0 eor, ;;
code: dp-imm/rot-sub r1 42 # r0 sub, ;;
code: dp-imm/rot-rsb r1 42 # r0 rsb, ;;
code: dp-imm/rot-add r1 42 # r0 add, ;;
code: dp-imm/rot-adc r1 42 # r0 adc, ;;
code: dp-imm/rot-sbc r1 42 # r0 sbc, ;;
code: dp-imm/rot-rsc r1 42 # r0 rsc, ;;
code: dp-imm/rot-orr r1 42 # r0 orr, ;;
code: dp-imm/rot-bic r1 42 # r0 bic, ;;

\ data processing immediate shift

code: dp-imm/sh-add0 r12 r11 9 #lsl r10 add, ;;
code: dp-imm/sh-add1 r12 r11 9 #lsr r10 add, ;;
code: dp-imm/sh-add2 r12 r11 9 #asr r10 add, ;;
code: dp-imm/sh-add3 r12 r11 9 #ror r10 add, ;;

code: dp-imm/sh-add-lsl0 r12 r11 r10 add, ;;
cr \ @@ fix asm.fs for the following two tests
0 $E08CA02B disasm-inst cr \ code: dp-imm/sh-add-lsr0 r12 r11 32 #lsr r10 add, ;;
0 $E08CA04B disasm-inst cr \ code: dp-imm/sh-add-asr0 r12 r11 32 #asr r10 add, ;;
code: dp-imm/sh-add-ror0 r12 r11 rrx r10 add, ;;

code: dp-imm/sh-mov0 r11 r10 mov, ;;
code: dp-imm/sh-mov1 r11 9 #lsl r10 mov, ;;
code: dp-imm/sh-mov2 r11 9 #lsl r10 movs, ;;
code: dp-imm/sh-mov3 r11 9 #lsl r10 ne movs, ;;

code: dp-imm/sh-cmp0 r11 r10 cmp, ;;
code: dp-imm/sh-cmp1 r11 r10 9 #lsl cmp, ;;
code: dp-imm/sh-cmp2 r11 r10 9 #lsl cmpp, ;;

\ data processing register shift

code: dp-reg/sh-sub0 r6 r5 r4 lsl r3 sub, ;;
code: dp-reg/sh-sub1 r6 r5 r4 lsr r3 sub, ;;
code: dp-reg/sh-sub2 r6 r5 r4 asr r3 sub, ;;
code: dp-reg/sh-sub3 r6 r5 r4 ror r3 sub, ;;

code: dp-reg/sh-mvn0 r5 r4 lsl r3 mvn, ;;
code: dp-reg/sh-mvn1 r5 r4 lsl r3 mvns, ;;

code: dp-reg/sh-teq0 r6 r5 r4 lsl teq, ;;
code: dp-reg/sh-teq1 r6 r5 r4 lsl teqp, ;;

code: dp-imm/rot-add0 r1 $3F0 # r0 add, ;;
code: dp-imm/rot-add1 r1 $3F0 # r0 adds, ;;

\ data processing immediate

code: dp-imm/rot-mov 42 # r0 mov, ;;
code: dp-imm/rot-mvn 42 # r0 mvn, ;;

code: dp-imm/rot-movs 42 # r0 movs, ;;
code: dp-imm/rot-mvns 42 # r0 mvns, ;;

code: dp-imm/rot-tst r1 42 # tst, ;;
code: dp-imm/rot-teq r1 42 # teq, ;;
code: dp-imm/rot-cmp r1 42 # cmp, ;;
code: dp-imm/rot-cmn r1 42 # cmn, ;;

code: dp-imm/rot-tstp r1 42 # tstp, ;;
code: dp-imm/rot-teqp r1 42 # teqp, ;;
code: dp-imm/rot-cmpp r1 42 # cmpp, ;;
code: dp-imm/rot-cmnp r1 42 # cmnp, ;;

\ load/store immediate offset

code: ls-imm-ldr0 r8       ]  r7 ldr, ;;
code: ls-imm-ldr1 r8    0 #]  r7 ldr, ;;
code: ls-imm-ldr2 r8   42 #]  r7 ldr, ;;
code: ls-imm-ldr3 r8  -42 #]  r7 ldr, ;;
code: ls-imm-ldr4 r8   42 #]! r7 ldr, ;;
code: ls-imm-ldr5 r8  -42 #]! r7 ldr, ;;
code: ls-imm-ldr6 r8   42 ]#  r7 ldr, ;;
code: ls-imm-ldr7 r8  -42 ]#  r7 ldr, ;;
code: ls-imm-ldr8 r8 $FFF ]#  r7 ldr, ;;

code: ls-imm0 r8 ] r7 str, ;;
code: ls-imm1 r8 ] r7 strb, ;;
\ @@ T forms depend on fix to asm.fs
code: ls-imm2 r8 0 ]# r7 ldrt, ;;   \ T forms require explicit
code: ls-imm3 r8 0 ]# r7 ldrbt, ;;  \ post-indexed addressing

\ load/store register offset

code: ls-reg-str0 r15 r14 9 #lsl +] r13 str, ;;
code: ls-reg-str1 r15 r14 9 #lsl -] r13 str, ;;
code: ls-reg-str2 r15 r14 9 #lsl +]! r13 str, ;;
code: ls-reg-str3 r15 r14 9 #lsl -]! r13 str, ;;
code: ls-reg-str4 r15 r14 9 #lsl ]+ r13 str, ;;
code: ls-reg-str5 r15 r14 9 #lsl ]- r13 str, ;;

code: ls-reg0 r3 r2 1 #lsl +] r0 ldr, ;;
code: ls-reg1 r3 r2 1 #lsl -] r0 ldrb, ;;
code: ls-reg2 r3 r2 1 #lsl ]+ r0 ldrt, ;;
code: ls-reg3 r3 r2 1 #lsl ]- r0 ldrbt, ;;

\ load/store multiple

code: ldm0 r0 da  { r1 r2 r3 } ldm, ;;
code: ldm1 r0 ia  { r1 r2 r3 } ldm, ;;
code: ldm2 r0 db  { r1 r2 r3 } ldm, ;;
code: ldm3 r0 ib  { r1 r2 r3 } ldm, ;;
code: ldm4 r0 da! { r1 r2 r3 } ldm, ;;
code: ldm5 r0 ia! { r1 r2 r3 } ldm, ;;
code: ldm6 r0 db! { r1 r2 r3 } ldm, ;;
code: ldm7 r0 ib! { r1 r2 r3 } ldm, ;;

code: stm0 r0 da  { r1 r2 r3 } stm, ;;

code: ^ldm0 r0 da { r2 r3 r4 } ^ldm, ;;

code: ^stm0 r0 da { r2 r3 r4 } ^stm, ;;

code: stm-all
    r0 da { r0 r1 r2 r3 r4 r5 r6 r7 r8 r9 r10 r11 r12 r13 r14 r15 } stm, ;;

\ software interrupt

code: swi0 $0 swi, ;;
code: swi1 $FFFFFF swi, ;;

\ multiply instructions

code: mul0 r3 r2 r1 mul, ;;
code: mul1 r3 r2 r1 muls, ;;

code: mla0 r4 r3 r2 r1 mla, ;;
code: mla1 r4 r3 r2 r1 mlas, ;;

code: smull0 r4 r3 r2 r1 smull, ;;
code: smull1 r4 r3 r2 r1 smulls, ;;
code: umull2 r4 r3 r2 r1 umull, ;;
code: umull3 r4 r3 r2 r1 umulls, ;;

code: smlal0 r4 r3 r2 r1 smlal, ;;
code: smlal1 r4 r3 r2 r1 smlals, ;;
code: umlal2 r4 r3 r2 r1 umlal, ;;
code: umlal3 r4 r3 r2 r1 umlals, ;;

\ branch, branch with link, branch with link and change to thumb

cr
$CAFE $EA000000 disasm-inst space \ +8 b
$CAFE $EAFFFFFF disasm-inst space \ +4 b
$CAFE $EAFFFFFE disasm-inst space \  0 b
$CAFE $EAFFFFFC disasm-inst space \ -8 b
cr

cr
$CAFE $0AFFFFFE disasm-inst space \  0 eq b
$CAFE $1AFFFFFE disasm-inst space \  0 ne b
$CAFE $BAFFFFFE disasm-inst space \  0 lt b
cr

cr
$CAFE $EB000000 disasm-inst space \ +8 bl
$CAFE $EBFFFFFE disasm-inst space \  0 bl
$CAFE $EBFFFFFC disasm-inst space \ -8 bl
cr

cr
$CAFE $FA000000 disasm-inst space \ +8 blx (h=0)
$CAFE $FAFFFFFE disasm-inst space \  0 blx (h=0)
$CAFE $FAFFFFFC disasm-inst space \ -8 blx (h=0)
cr

cr
$CAFE $FB000000 disasm-inst space \ +A blx (h=1)
$CAFE $FBFFFFFE disasm-inst space \  0 blx (h=1)
$CAFE $FBFFFFFC disasm-inst space \ -A blx (h=1)
cr

code: blx0  here $10 +  #blx ;;   \ +8 blx (h=0)
code: blx1  here $8  +  #blx ;;   \  0 blx (h=0)
code: blx2  here 0   +  #blx ;;   \ -8 blx (h=0)

code: blx3  here $12 +  #blx ;;   \ +A blx (h=1)
code: blx4  here $A  +  #blx ;;   \  0 blx (h=1)
code: blx5  here 2   +  #blx ;;   \ -A blx (h=1)

\ FPA extension

cr
$0 $ED948102 disasm-inst cr \ r4 8 #] f0 ldfd,
$0 $ED949100 disasm-inst cr \ r4    ] f1 ldfd,
$0 $EE000181 disasm-inst cr \ f1   f0 f0 adfd,
$0 $ED848102 disasm-inst cr \ r3 8 #] f0 stfd,
$0 $EC848102 disasm-inst cr \ r3 $8 ]$ f0 stfd,

$0 $EE00018D disasm-inst cr \ $5+0 # f0 f0 adfd,
$0 $EE00018E disasm-inst cr \ $5-1 # f0 f0 adfd,
$0 $EE00018F disasm-inst cr \ $A+0 # f0 f0 adfd,

$0 $4E008181 disasm-inst cr \ f1 f0 mi mvfd,

$0 $EE90F110 disasm-inst cr \ f0 f0 cmf,
$0 $EEB0F118 disasm-inst cr \ f0 0.0 # cnf,
$0 $EED4F118 disasm-inst cr \ f4 0.0 # cmfe,

$0 $ECBDC203 disasm-inst cr \ 1 r14 ]# f4 lfm,  ( lfm f4, 1, [sp], #12 )

$0 $ED948F02 disasm-inst cr \ no ldfd, but ldc
$0 $EE000F81 disasm-inst cr \ no adfd, but cdp

\ instructions supported by the assembler, but not the disassembler

code: misc0 r1 ] r0 ldrh, ;;
code: misc1 r1 ] r0 ldrsh, ;;
code: misc2 r1 ] r0 ldrsb, ;;
code: misc3 r2 1 #] r0 strh, ;;
code: misc4 r2 r1 +] r0 strh, ;;

code: misc5 r2 ] r1 r0 swp, ;;
code: misc6 r2 ] r1 r0 swpb, ;;

code: misc7 r0 cpsr mrs, ;;
code: misc8 r0 spsr mrs, ;;

code: misc9   cpsr c x s f  r0 msr, ;;
code: misc10  spsr c x s f  $0ff # msr, ;;

\ --

previous

cr .s cr
