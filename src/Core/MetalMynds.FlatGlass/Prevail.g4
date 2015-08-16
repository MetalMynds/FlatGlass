grammar Prevail;

/*
 * Parser Rules
 */

expression
	|   condition 
	:	EOF
	;

condition 
	| (AND condition+) -> AndCondition
	| (OR condition+) -> OrCondition
	| (NOT condition+) -> NotCondition
	| (IDENTIFIER EQUAL value) -> PropertyCondition


value : (number | STRING);

number
	: (INT | FLOAT)
	;

INT
	: Digit+
	;

fragment
Digit
	: [0-9]
	;

FLOAT
	: Digit+ '.' Digit* ExponentPart?
	| '.' Digit+ ExponentPart?
	| Digit+ ExponentPart
	;

fragment
ExponentPart
	: [eE] [+-]? Digit+
	;

/*
 * Lexer Rules
 */

AND : 'And' | 'AND' | 'and' | '&&';

OR : 'Or' | 'OR' | 'or' | '||';

IDENTIFIER
	: [a-zA-Z_][a-zA-Z_0-9]*
	;

STRING : '"' ( '\\"' | . )*? '"' ;

EQUAL : '=' | '==' | 'EQUAL' | 'Equal' | 'equals';

NOT_EQUAL : '!=' | '!=' | '!==';
	
NEWLINE
	: '\r'? '\n' -> skip
	;

DOT : '.';

COMMA : ',';

WS
	:	[ \t\u000C]+ -> channel(HIDDEN)
	;
