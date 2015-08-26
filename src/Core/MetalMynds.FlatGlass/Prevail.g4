grammar Prevail;

/*
 * Parser Rules
 */

expression
		:   (condition)* NEWLINE*
		|   EOF
		;

condition 
		: '(' condition ')' # SubCondition
		| (property AND condition) # AndCondition
		| (property OR condition) # OrCondition
		| (property NOT condition) # NotCondition                
		|  property # PropertyCondition
		;

property 
		: (IDENTIFIER (EQUAL | NOT_EQUAL) value);
   
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

NOT: 'Not' | 'NOT' | 'not';

IDENTIFIER
	: [a-zA-Z_][a-zA-Z_0-9]*
	;

STRING : QUOTE ('\\'QUOTE | . )*? QUOTE;

fragment QUOTE
	:   '\'';

EQUAL : '=' | '==' | 'EQUAL' | 'Equal' | 'equals';

NOT_EQUAL : '!=' | '!==' | '!===' | '<>';
	
NEWLINE
	: '\r'? '\n' -> skip
	;

DOT : '.';

COMMA : ',';

WS
	:	[ \t\u000C]+ -> channel(HIDDEN)
	;
