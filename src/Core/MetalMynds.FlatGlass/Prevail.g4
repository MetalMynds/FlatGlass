grammar Prevail;

/*
 * Parser Rules
 */

condition
	:	EOF
	;

/*
 * Lexer Rules
 */

WS
	:	' ' -> channel(HIDDEN)
	;
