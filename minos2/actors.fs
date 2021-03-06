\ MINOS2 actors basis

\ Copyright (C) 2017 Free Software Foundation, Inc.

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

\ actors are responding to any events that need to be handled

\ actor handler class

\ platform specific action handler

\ edit actor

edit-terminal-c class
    cell uvar edit$ \ pointer to the edited string
end-class edit-widget-c

edit-widget-c ' new static-a with-allocater Constant edit-widget

[IFDEF] x11      include x11-actors.fs      [THEN]
[IFDEF] wayland  include wayland-actors.fs  [THEN]
[IFDEF] android  include android-actors.fs  [THEN]

\ generic actor stuff

actor class
end-class outside-actor
:noname ( rx ry -- flag )
    fdrop fdrop false ; outside-actor is inside?

: !act ( o:widget actor -- o:widget )
    to act o act >o to caller-w o> ;
: outside[] ( o -- o )
    >o outside-actor new !act o o> ;

actor class
end-class simple-actor

: simple-inside? ( rx ry -- flag )
    caller-w >o
    y f- fdup d f< h fnegate f> and
    x f- fdup w f< 0e f> and
    and o> ;
' simple-inside? simple-actor is inside?

debug: event( \ +db event( \ )
:noname { f: rx f: ry b n -- }
    event( o hex. caller-w hex. ." simple click: " rx f. ry f. b . n . cr ) ; simple-actor is clicked
:noname ( addr u -- )
    event( o hex. caller-w hex. ." keyed: " type cr )else( 2drop ) ; simple-actor is ukeyed
:noname ( ekey -- )
    event( o hex. caller-w hex. ." ekeyed: " hex. cr )else( drop ) ; simple-actor is ekeyed
: .touch ( $xy b -- )
    event( ." touch: " hex. $@ bounds ?DO  I sf@ f.  1 sfloats +LOOP cr )else( 2drop ) ;
:noname ( $xy b -- )
    event( o hex. caller-w hex. ." down " .touch )else( 2drop )
; simple-actor is touchdown
:noname ( $xy b -- )
    event( o hex. caller-w hex. ." up " .touch )else( 2drop )
; simple-actor is touchup
:noname ( $xy b -- )
    event( o hex. caller-w hex. ." move " .touch )else( 2drop )
; simple-actor is touchmove

: simple[] ( o -- o )
    >o simple-actor new !act o o> ;

\ click actor

simple-actor class
    method do-action
    defer: ck-action
    value: data
end-class click-actor

' ck-action click-actor is do-action

: click[] ( o xt data -- o )
    rot >o click-actor new >o to data is ck-action o o> !act o o> ;

:noname ( rx ry b n -- )
    fdrop fdrop 1 and 0= swap 1 <= and IF  do-action  THEN
; click-actor is clicked
:noname ( ukeyaddr u -- )
    bounds ?DO  I c@ bl = IF  do-action  THEN
    LOOP ; click-actor is ukeyed
:noname ( ekey -- )
    k-enter = IF  do-action  THEN
; click-actor is ekeyed

\ toggle actor

click-actor class
end-class toggle-actor

: toggle[] ( o xt state -- o )
    rot >o toggle-actor new >o to data is ck-action o o> !act o o> ;

:noname data 0= dup to data ck-action ; toggle-actor is do-action

\ actor for a box with one active element

actor class
end-class box-actor

false value grab-move? \ set to true to grab moves
0 value select-mode    \ 0: chars, 1: words, 2: lines
0 value start-cursize \ selection helper

: re-focus { c-act -- }
    c-act .active-w ?dup-IF  .act ?dup-IF  .defocus  THEN  THEN
    o c-act >o to active-w o>
    c-act .active-w ?dup-IF  .act ?dup-IF  .focus  THEN  THEN ;

: engage ( object -- )
    >o parent-w ?dup-IF
	recurse parent-w .act re-focus  THEN  o> ;

:noname ( rx ry b n -- )
    event( o hex. caller-w hex. ." box click: " fover f. fdup f. over . dup . cr )
    grab-move? IF
	active-w ?dup-IF  .act .clicked  EXIT  THEN
    THEN
    fover fover inside? IF
	o caller-w >o
	[: { c-act } act IF  fover fover act .inside?
		IF
		    c-act re-focus
		    fover fover 2dup act .clicked  THEN  THEN
	c-act ;] do-childs o> drop
    THEN  2drop fdrop fdrop ;
box-actor is clicked
:noname ( addr u -- )
    active-w ?dup-IF  .act .ukeyed  ELSE  2drop  THEN ; box-actor is ukeyed
:noname ( ekey -- )
    active-w ?dup-IF  .act .ekeyed  ELSE  drop   THEN ; box-actor is ekeyed
' simple-inside? box-actor is inside?
: xy@ ( addr -- rx ry )  $@ drop dup sf@ sfloat+ sf@ ;
:noname ( $xy b -- )
    over xy@ inside? IF
	caller-w >o
	[: act IF  over xy@ act .inside?
		IF  2dup act .touchdown   THEN  THEN
	;] do-childs o>
    THEN  2drop ; box-actor is touchdown
:noname ( $xy b -- )
    over xy@ inside? IF
	caller-w >o
	[: act IF  over xy@ act .inside?
		IF  2dup act .touchup   THEN  THEN
	;] do-childs o>
    THEN  2drop ; box-actor is touchup
:noname ( $xy b -- )
    event( o hex. caller-w hex. ." box move " 2dup .touch )
    grab-move? IF
	active-w ?dup-IF  .act .touchmove  EXIT  THEN
    THEN
    over xy@ inside? IF
	caller-w >o
	[: act IF  over xy@ act .inside?
		event( o hex. caller-w hex. ." move inside? " dup . cr )
		IF  2dup act .touchmove  THEN  THEN
	;] do-childs o>
    THEN  2drop ; box-actor is touchmove
:noname ( -- ) caller-w >o [: act ?dup-IF  .defocus  THEN ;] do-childs o> ; box-actor is defocus

: box[] ( o -- o )
    >o box-actor new !act o o> ;

\ viewport

box-actor class
    field: txy$ \ translated xy$
end-class vp-actor

:noname caller-w >o vp-h f< vp-w f< and o> ; vp-actor is inside?

: tx ( rx ry -- rx' ry' )
    fswap x f- vp-x         f+
    fswap y f- vp-h vp-y f- f+ ;
: tx$ ( $rxy*n -- $rxy*n' )
    0e fdup tx { f: dx f: dy }
    dup $@len act .txy$ $!len
    act .txy$ $@ drop swap $@ bounds U+DO
	I         sf@ dx f+ sf!+
	I sfloat+ sf@ dy f+ sf!+
    [ 2 sfloats ]L +LOOP  drop  act .txy$ ;

: vp-need-or ( -- )
    vp-need @ need-mask @ over $FF and over $FF and or >r
    $-100 and swap $-100 and max r> or need-mask ! ;

:noname ( rx ry bmask n -- )
    caller-w >o
    tx [: act >o [ box-actor :: clicked ] o> ;] vp-needed
    vp-need-or o> ; vp-actor is clicked
:noname ( $rxy*n bmask -- )
    caller-w >o
    >r tx$ r> [: act >o [ box-actor :: touchdown ] o> ;] vp-needed
    vp-need-or o> ; vp-actor is touchdown
:noname ( $rxy*n bmask -- )
    caller-w >o
    >r tx$ r> [: act >o [ box-actor :: touchup ] o> ;] vp-needed
    vp-need-or o> ; vp-actor is touchup
:noname ( $rxy*n bmask -- )
    caller-w >o
    >r tx$ r> [: act >o [ box-actor :: touchmove ] o> ;] vp-needed
    vp-need-or o> ; vp-actor is touchmove
:noname ( ekey -- )
    caller-w >o
    [: act >o [ box-actor :: ekeyed ] o> ;] vp-needed
    vp-need-or o> ; vp-actor is ekeyed
:noname ( ekey -- )
    caller-w >o
    [: act >o [ box-actor :: ukeyed ] o> ;] vp-needed
    vp-need-or o> ; vp-actor is ukeyed

: vp[] ( o -- o )
    >o vp-actor new !act o o> ;

\ slider actor

simple-actor class
    value: slide-vp
    fvalue: slider-sxy
end-class hslider-actor

hslider-actor class
end-class vslider-actor

: >hslide ( x -- )
    slider-sxy f- caller-w >o parent-w .w w f- +sync o> f/
    slide-vp >o vp-w w f- fdup { f: hmax } f*
    0e fmax hmax fmin fround to vp-x
    ?vpt-x IF  ['] +sync vp-needed  THEN o>
    caller-w .parent-w >o !size xywhd resize o> ;

:noname ( $rxy*n bmask -- ) 
    grab-move? IF
	drop xy@ fdrop >hslide
    ELSE
	2drop
    THEN ; hslider-actor is touchmove
:noname ( x y b n -- )
    1 and IF
	drop fdrop caller-w .parent-w .childs[] $@ drop @ .w f- to slider-sxy
	true to grab-move?
    ELSE
	drop fdrop >hslide
	false to grab-move?
    THEN ; hslider-actor is clicked

: hslider[] ( vp o -- )
    >o o hslider-actor new to act
    act >o to caller-w to slide-vp -1e to slider-sxy o> o> ;

: >vslide ( x -- )
    slider-sxy fswap f- caller-w >o parent-w .h h f- +sync o> f/
    slide-vp >o vp-h h f- fdup { f: vmax } f*
    0e fmax vmax fmin fround to vp-y
    ?vpt-y IF  ['] +sync vp-needed  THEN  o>
    caller-w .parent-w >o !size xywhd resize o> ;

:noname ( $rxy*n bmask -- )
    event( o hex. caller-w hex. ." slider move " 2dup .touch )
    grab-move? IF
	drop xy@ fnip >vslide
    ELSE  2drop  THEN ; vslider-actor is touchmove
:noname ( x y b n -- )
    event( o hex. caller-w hex. ." slider click " fover f. fdup f. over . dup . cr )
    1 and IF
	drop fnip caller-w .parent-w .childs[] $@ cell- + @ .h f+
	to slider-sxy
	true to grab-move?
    ELSE
	drop fnip >vslide
	false to grab-move?
    THEN ; vslider-actor is clicked

: vslider[] ( vp o -- )
    >o o vslider-actor new to act
    act >o to caller-w to slide-vp -1e to slider-sxy o> o> ;

\ edit widget

: grow-edit$ { max span addr pos1 more -- max span addr pos1 true }
    max span more + u> IF  max span addr pos1 true  EXIT  THEN
    span more + edit$ @ $!len
    edit$ @ $@ swap span swap pos1 true ;

edit-widget edit-out !

bl cells buffer: edit-ctrlkeys
xchar-ctrlkeys edit-ctrlkeys bl cells move
keycode-limit keycode-start - cells buffer: edit-ekeys
std-ekeys edit-ekeys keycode-limit keycode-start - cells move

' edit-ctrlkeys is ctrlkeys
' edit-ekeys is ekeys
' grow-edit$ is grow-tib
[IFDEF] android
    also jni
    : android-seteditline ( span addr pos -- span addr pos )
	2dup xcs swap >r >r
	2dup swap make-jstring r> clazz .setEditLine r>
	+sync ;
    previous
    ' android-seteditline is edit-update
[ELSE]
    ' noop is edit-update \ no need to do that here
[THEN]
' noop is edit-error  \ no need to make annoying bells
' clipboard!     is paste!
[IFUNDEF] primary!     ' clipboard! alias primary! [THEN]
[IFUNDEF] primary@     ' clipboard@ alias primary@ [THEN]

\ extra key bindings for editors

simple-actor class
    method edit-next-line
    method edit-prev-line
    value: edit-w
    defer: edit-enter
end-class edit-actor

' false edit-actor is edit-next-line
' false edit-actor is edit-prev-line

0 value xselw

: edit-copy ( max span addr pos1 -- max span addr pos1 false )
    >r 2dup swap r@ safe/string xselw min clipboard!
    r> 0 ;
: edit-cut ( max span addr pos1 -- max span addr pos1 false )
    edit-copy drop >r
    2dup swap r@ safe/string xselw delete
    swap xselw - swap
    r> edit-update 0 ;
: edit-bs ( max span addr pos1 -- max span addr pos1 false )
    xselw 0> IF  edit-cut  ELSE  ?xdel  THEN ;
: edit-del ( max span addr pos1 -- max span addr pos1 false )
    xselw 0> IF  edit-cut  ELSE  <xdel>  THEN ;

Defer anim-ins

: edit-ins$ ( max span addr pos1 addr u -- max span' addr pos1' )
    anim-ins
    xselw 0> IF  save-mem 2>r edit-cut drop 2r@ insert-string
	2r> drop free throw
    ELSE  insert-string  THEN ;

: edit-paste ( max span addr pos1 - max span addr pos2 false )
    clipboard@ edit-ins$ edit-update 0 ;

' edit-next-line ctrl N bindkey
' edit-prev-line ctrl P bindkey
' edit-paste     ctrl V bindkey
' edit-paste     ctrl Y bindkey
' edit-copy      ctrl C bindkey
' edit-cut       ctrl X bindkey
' edit-cut       ctrl W bindkey
' edit-enter     #lf    bindkey
' edit-enter     #cr    bindkey
' false          ctrl L bindkey
' edit-bs        ctrl H bindkey
' edit-del       ctrl D bindkey

' edit-next-line k-down   ebindkey
' edit-prev-line k-up     ebindkey
' edit-next-line k-next   ebindkey
' edit-prev-line k-prior  ebindkey
' edit-enter     k-eof    ebindkey
' edit-enter     k-enter  ebindkey
' false          k-winch  ebindkey
' edit-del       k-delete ebindkey

edit-terminal edit-out !

: *xins        anim-ins  defers insert-char ;

' *xins        is insert-char

\ edit things

: edit-xt ( ... xt o:actor -- )
    \G pass xt to the editor
    \G xt has ( ... addr u curpos cursize -- addr u curpos cursize ) as stack effect
    *insflag off
    history >r  >r  0 to history
    edit-w >o addr text$ curpos cursize 0 max o> to xselw
    >r dup edit$ ! dup { e$ } $@ swap over swap r>
    r> catch >r edit-w >o to curpos 0 to cursize o> drop e$ $!len drop
    r>  r> to history  +sync +config  throw ;

: edit>curpos ( x o:actor -- )
    edit-w >o  text-font to font
    x f- border f- w border f2* f- text-w f/ f/
    text$ pos-string to curpos
    o>  +sync ;

[IFUNDEF] -scan
    : -scan ( addr u char -- addr' u' )
	>r  BEGIN  dup  WHILE  1- 2dup + c@ r@ =  UNTIL  THEN
	rdrop ;
[THEN]
: select-word ( o:edit-w -- )
    start-curpos 0< IF  curpos to start-curpos  THEN
    text$ start-curpos start-cursize + curpos cursize + umax safe/string
    bl scan drop { end }
    text$ drop start-curpos curpos umin bl -scan + dup c@ bl = - { start }
    start text$ drop - to curpos
    end start - to cursize +sync ;
: select-line ( o:edit-w -- )
    0 to curpos text$ nip to cursize +sync ;
: sel>primary ( o:edit-w -- )
    text$ curpos safe/string cursize min 0 max primary! ;
: end-selection ( o:edit-w -- )
    start-curpos 0>= IF
	curpos start-curpos 2dup - abs to cursize +sync
	umin to curpos
	case select-mode
	    1 of select-word endof
	    2 of select-line endof
	endcase
	-1 to start-curpos
    THEN ;
: start-selection ( fx fy b n -- )
    *insflag off
    edit-w .start-curpos 0< IF
	1- 2/ to select-mode
	drop fdrop edit>curpos  edit-w >o
	0 to cursize +sync
	curpos  to start-curpos
	cursize to start-cursize
	case select-mode
	    1 of select-word endof
	    2 of select-line endof
	endcase
	curpos  to start-curpos
	cursize to start-cursize
	o>
	true to grab-move?
    ELSE
	false to grab-move?
	2drop fdrop fdrop
    THEN ;
: expand-selection ( $xy -- )
    edit-w .start-curpos 0>= IF
	xy@ fdrop edit>curpos
	edit-w >o
	curpos start-curpos 2dup - abs to cursize +sync
	umin to curpos
	case  select-mode
	    1 of select-word endof
	    2 of select-line endof
	endcase
	o>
    ELSE  drop
    THEN ;

:noname ( key o:actor -- )
    [: 4 roll dup $80000000 and 0= k-ctrl-mask and invert and
	>control edit-control drop ;] edit-xt ; edit-actor is ekeyed
:noname ( addr u o:actor -- )
    [: 2rot edit-ins$ edit-update ;] edit-xt ; edit-actor is ukeyed
:noname ( o:actor -- )
    edit-w >o -1 to cursize o> +sync
    -keyboard ; edit-actor is defocus
:noname ( o:actor -- )
    edit-w >o  0 to cursize o> +sync
    +keyboard ; edit-actor is focus
:noname ( $rxy*n bmask -- )
    case
	0 of  expand-selection  endof
	1 of  expand-selection  endof
	nip
    endcase
; edit-actor is touchmove
:noname ( o:actor rx ry b n -- )
    dup 1 and IF  start-selection
    ELSE
	false to grab-move?
	swap
	case
	    dup 1 u<= ?of drop
		2 - 6 mod 2 +
		{ clicks } fdrop edit>curpos
		edit-w >o
		case clicks
		    2 of  end-selection  endof
		    4 of  select-word    endof
		    6 of  select-line    endof
		endcase
		sel>primary
		-1 to start-curpos
		0  to start-cursize
		o>
	    endof
	    2 of  drop fdrop edit>curpos
		[: primary@ edit-ins$ ;] edit-xt  endof
	    4 of  ( menu   )  drop fdrop fdrop  endof
	    nip fdrop fdrop
	endcase
    THEN
; edit-actor is clicked

: edit[] ( o widget xt -- o ) { xt }
    swap >o edit-actor new to act
    o act >o to caller-w to edit-w xt is edit-enter o>
    o o> ;
